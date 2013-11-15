using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Cqrs.Commands.Reservation
{
    [Serializable]
    public class CancelReservation : CommandBase
    {
        public Guid ReservationId { get; set; }
        public String Reason { get; set; }
    }
}
