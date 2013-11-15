using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Cqrs.Commands.Writer
{
    public interface ICommandWriter
    {
        void SendCommand(CommandBase theCommand);
    }
}
