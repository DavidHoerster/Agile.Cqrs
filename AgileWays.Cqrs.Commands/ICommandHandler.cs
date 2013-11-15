using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Cqrs.Commands
{
    [ServiceContract()]
    public interface ICommandHandler
    {
        [OperationContract(IsOneWay = true)]
        void Handle(CommandBase command);
    }
}
