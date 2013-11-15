using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcWebRole1
{
    public class AzureCommandWriter : AgileWays.Cqrs.Commands.Writer.ICommandWriter
    {
        public void SendCommand(AgileWays.Cqrs.Commands.CommandBase theCommand)
        {
            CloudHelper.EnqueueCommand(theCommand);
        }
    }
}