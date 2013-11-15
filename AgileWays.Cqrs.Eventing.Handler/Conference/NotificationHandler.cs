using AgileWays.Cqrs.Events.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Cqrs.Eventing.Handler.Conference
{
    public class NotificationHandler : IHandle<ReservationCanceled>
    {
        public void Handle(ReservationCanceled theEvent)
        {
            Console.WriteLine("Oh snap!  Someone just canceled.");
        }
    }
}
