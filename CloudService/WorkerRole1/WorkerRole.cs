using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using AgileWays.Cqrs.Commanding;
using AgileWays.Cqrs.Commands;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private static CloudQueue _itemQueue;
        public static CloudQueue CommandQueue
        {
            get
            {
                return _itemQueue;
            }
            internal set
            {
                _itemQueue = value;
            }
        }

        private static CloudBlobContainer _itemBlobContainer;
        public static CloudBlobContainer CommandBlobContainer
        {
            get
            {
                return _itemBlobContainer;
            }
            internal set
            {
                _itemBlobContainer = value;
            }
        }

        private static CloudQueue _logQueue;
        public static CloudQueue LogQueue
        {
            get
            {
                return _logQueue;
            }
            internal set
            {
                _logQueue = value;
            }
        }

        private static CloudBlobContainer _logBlobContainer;
        public static CloudBlobContainer LogBlobContainer
        {
            get
            {
                return _logBlobContainer;
            }
            internal set
            {
                _logBlobContainer = value;
            }
        }

        private static CommandHandler _service;

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("WorkerRole1 entry point called", "Information");

            Timer logTimer = new Timer(new TimerCallback(LogQueuePoller), null, 0, 10000);

            while (true)
            {
                try
                {
                    CommandQueuePoller(_service);
                    Thread.Sleep(2000);     //sleep for 2 seconds
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Error occurred: " + e.Message);
                    if (e.InnerException != null)
                    {
                        Trace.WriteLine("Additional error message: " + e.InnerException.Message);
                        Trace.WriteLine("Stack: " + e.InnerException.StackTrace);
                    }
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) => configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)));
            CloudStorageAccount storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

            InitializeStorageResources(storageAccount);

            _service = new CommandHandler();

            return base.OnStart();
        }

        private static void InitializeStorageResources(CloudStorageAccount storageAccount)
        {
            var blobStorage = storageAccount.CreateCloudBlobClient();
            LogBlobContainer = blobStorage.GetContainerReference("logitems");
            CommandBlobContainer = blobStorage.GetContainerReference("commands");

            var queueStorage = storageAccount.CreateCloudQueueClient();
            LogQueue = queueStorage.GetQueueReference("logmessages");
            CommandQueue = queueStorage.GetQueueReference("commandmessages");

            bool isStorageFinalized = false;
            while (!isStorageFinalized)
            {
                try
                {
                    LogBlobContainer.CreateIfNotExist();
                    LogQueue.CreateIfNotExist();
                    CommandBlobContainer.CreateIfNotExist();
                    CommandQueue.CreateIfNotExist();

                    isStorageFinalized = true;
                }
                catch (StorageClientException sce)
                {
                    if (sce.ErrorCode == StorageErrorCode.TransportError)
                    {
                        Trace.TraceError("Storage services initialization failure.  " +
                            "Check your storage account configuration settings.  " +
                            "If running locally, ensure that the Development Storage service is running.  " +
                            "Message: '{0}'", sce.Message);
                    }
                    else
                    {
                        throw sce;
                    }
                }
            }
        }


        public static void CommandQueuePoller(CommandHandler service)
        {
            //Trace.WriteLine("LogPoller has started -- seeing if there's anything in the queue for me!!");

            CloudQueueMessage msg = CommandQueue.GetMessage();
            if (msg == null)
            {
                //Trace.TraceInformation("COMMAND QUEUE nothing found - going back to sleep");
            }

            while (msg != null)
            {
                string myMessage = msg.AsString;
                Trace.TraceInformation("Got message {0}", myMessage);

                //probably should do some checking here before deleting...
                CommandQueue.DeleteMessage(msg);

                //split the ID from the type -- first item is ID, second is type
                string[] messageParts = myMessage.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (messageParts.Length != 2)
                {
                    Trace.TraceError("COMMAND QUEUE message improperly formed.  {0}", myMessage);
                    continue;
                }


                CloudBlockBlob theBlob = CommandBlobContainer.GetBlockBlobReference(messageParts[0]);
                Type commandType = Type.GetType(messageParts[1]);
                CommandBase theCommand = null;

                using (MemoryStream msBlob = new MemoryStream())
                {
                    byte[] logBytes = theBlob.DownloadByteArray();
                    msBlob.Write(logBytes, 0, logBytes.Length);
                    BinaryFormatter bf = new BinaryFormatter();
                    msBlob.Position = 0;
                    object theObject = bf.Deserialize(msBlob);
                    theCommand = Convert.ChangeType(theObject, commandType) as CommandBase;
                }

                if (theCommand != null)
                {
                    service.Handle(theCommand);
                }
                else
                {
                    Trace.TraceInformation("COMMAND BLOB Could not deserialize message from queue id {0}", myMessage);
                }

                msg = CommandQueue.GetMessage();
            }
        }

        public static void LogQueuePoller(object state)
        {
            Trace.WriteLine("LogPoller has started -- seeing if there's anything in the queue for me!!");

            CloudQueueMessage msg = LogQueue.GetMessage();
            if (msg == null)
            {
                Trace.TraceInformation("LOG QUEUE nothing found - going back to sleep");
            }

            while (msg != null)
            {
                //do something
                string myMessage = msg.AsString;
                LogQueue.DeleteMessage(msg);
                Trace.TraceInformation("Got message {0}", myMessage);

                CloudBlockBlob theBlob = LogBlobContainer.GetBlockBlobReference(myMessage);
                AgileWays.Cqrs.Commands.Logging.LogCommand theLog;

                DeserializeObjectFromBlob<AgileWays.Cqrs.Commands.Logging.LogCommand>(theBlob, out theLog);

                if (theLog != null)
                {
                    WriteLogToStorage(myMessage, theLog);
                }
                else
                {
                    Trace.TraceInformation("Could not deserialize message from queue id {0}", myMessage);
                }

                msg = LogQueue.GetMessage();
            }
        }

        private static void WriteLogToStorage(string myMessage, AgileWays.Cqrs.Commands.Logging.LogCommand theLog)
        {
            Trace.TraceInformation("Message text {0}, sent {1} from user {2} on queue id {3}",
                theLog.Message,
                theLog.LogTime.ToString(),
                theLog.UserID.ToString(),
                myMessage);

            AgileWays.Repository.Logging.LogEntry log = new AgileWays.Repository.Logging.LogEntry(theLog.UserID)
            {
                LogDate = theLog.LogTime,
                Message = theLog.Message,
                MessageType = theLog.MessageType
            };

            AgileWays.Repository.Logging.LogEntryDataSource data = new AgileWays.Repository.Logging.LogEntryDataSource();
            data.AddLogEntry(log);
        }

        private static void DeserializeObjectFromBlob<T>(CloudBlockBlob theBlob, out T theObject)
            where T : class
        {
            using (MemoryStream msBlob = new MemoryStream())
            {
                byte[] logBytes = theBlob.DownloadByteArray();
                msBlob.Write(logBytes, 0, logBytes.Length);
                BinaryFormatter bf = new BinaryFormatter();
                msBlob.Position = 0;
                theObject = bf.Deserialize(msBlob) as T;
            }
        }

    }
}
