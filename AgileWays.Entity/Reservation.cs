﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Entity
{
    public class Reservation
    {
        public Guid ReservationId { get; set; }
        public DateTime ReservationMade { get; set; }
        public Int32 NumberOfSeats { get; set; }
        public String SeatsReservedFor { get; set; }
        public Decimal TotalCost { get; set; }
        public Boolean IsDeleted { get; set; }
        public String Reason { get; set; }
        public DateTime? CancellationDate { get; set; }
    }
}
