using AgileWays.Cqrs.Commands.Reservation;
using AgileWays.Cqrs.Events.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Cqrs.Domain.Conference
{
    public class Reservation : AggregateRootBase
    {
        private Decimal _ticketCost = 49.95m;
        public Reservation()
        {
        }

        public void CreateNewReservation(CreateReservation cmd)
        {
            Decimal discountFactor,
                    totalCost;
            if (cmd.DiscountCode=="FREE")
            {
                //it's free
                discountFactor = 0;
            }
            else if (cmd.DiscountCode=="HALF")
            {
                //half off
                discountFactor = 0.5m;
            }
            else
            {
                //too bad
                discountFactor = 1;
            }
            totalCost = cmd.NumberOfSeats * (_ticketCost * discountFactor);

            ApplyEvent(new ReservationCreated()
            {
                ReservationId = cmd.ReservationId,
                ReservationMade = DateTime.UtcNow,
                NumberOfSeats = cmd.NumberOfSeats,
                SeatsReservedFor = cmd.Name,
                TotalCost = totalCost
            });

        }

        public void CancelReservation(CancelReservation cmd)
        {
            var date = DateTime.UtcNow;

            ApplyEvent(new ReservationCanceled()
            {
                ReservationId = cmd.ReservationId,
                Reason = cmd.Reason,
                CancellationDate = date
            });
        }
    }
}
