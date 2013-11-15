using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileWays.Repository.Logging
{
    public class LogDataContext : TableServiceContext
    {
        public LogDataContext(string baseAddress, StorageCredentials creds)
            : base(baseAddress, creds)
        {
        }

        public IQueryable<LogEntry> LogEntry
        {
            get
            {
                return this.CreateQuery<LogEntry>("LogEntry");
            }
        }
    }
}
