using AgileWays.Cqrs.Events.Reservation;
using AgileWays.Entity;
using AgileWays.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Cqrs.Eventing.Handler.Conference
{
    public class ReservationHandler : IHandle<ReservationCreated>,
                                        IHandle<ReservationCanceled>
    {
        public void Handle(ReservationCreated theEvent)
        {
            var reservation = new Reservation()
            {
                IsDeleted = false,
                NumberOfSeats = theEvent.NumberOfSeats,
                ReservationId = theEvent.ReservationId,
                ReservationMade = theEvent.ReservationMade,
                SeatsReservedFor = theEvent.SeatsReservedFor,
                TotalCost = theEvent.TotalCost
            };

            var repo = new ReservationRepository();
            repo.CreateReservation(reservation);

            Console.WriteLine("reservation created for {0} at a cost of {1}", theEvent.SeatsReservedFor, theEvent.TotalCost.ToString());
        }

        public void Handle(ReservationCanceled theEvent)
        {
            Console.WriteLine("reservation canceled for id {0} at {1} because of {2}", theEvent.ReservationId.ToString(), theEvent.CancellationDate.ToString(), theEvent.Reason);

            var repo = new ReservationRepository();
            repo.CancelReservation(theEvent.ReservationId,
                                    theEvent.CancellationDate,
                                    theEvent.Reason);

        }
    }
}
