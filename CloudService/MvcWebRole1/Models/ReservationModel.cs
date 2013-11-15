using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcWebRole1.Models
{
    public class ReservationModel
    {
        public String Name { get; set; }
        public Int32 NumberOfSeats { get; set; }
        public String DiscountCode { get; set; }
    }
}