using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Cqrs.Commands.Reservation
{
    [Serializable]
    public class CreateReservation : CommandBase
    {
        public Guid ReservationId { get; set; }
        public String Name { get; set; }
        public Int32 NumberOfSeats { get; set; }
        public String DiscountCode { get; set; }
    }
}
