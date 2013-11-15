using AgileWays.Cqrs.Eventing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Cqrs.Events.Reservation
{
    [Serializable]
    public class ReservationCanceled : EventBase
    {
        public Guid ReservationId { get; set; }
        public DateTime CancellationDate { get; set; }
        public String Reason { get; set; }
    }
}
