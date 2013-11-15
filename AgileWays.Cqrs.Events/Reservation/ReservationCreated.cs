using AgileWays.Cqrs.Eventing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Cqrs.Events.Reservation
{
    [Serializable]
    public class ReservationCreated : EventBase
    {
        public Guid ReservationId { get; set; }
        public DateTime ReservationMade { get; set; }
        public Int32 NumberOfSeats { get; set; }
        public String SeatsReservedFor { get; set; }
        public Decimal TotalCost { get; set; }
    }
}
