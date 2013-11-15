using AgileWays.Cqrs.Commands.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var theCommand = new CreateReservation()
            {
                DiscountCode = "HALF",
                Name = "David Hoerster",
                NumberOfSeats = 2,
                ReservationId = Guid.NewGuid()
            };

            AgileWays.Cqrs.Commands.Writer.ICommandWriter writer = 
                new AgileWays.Cqrs.Commands.Writer.CommandWriter();
            writer.SendCommand(theCommand);

            var cancellation = new CancelReservation()
            {
                Reason = "just not that into it anymore",
                ReservationId = Guid.NewGuid()
            };
            writer.SendCommand(cancellation);
        }
    }
}
