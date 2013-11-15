using AgileWays.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Repository
{
    public class ReservationRepository
    {
        private readonly ReservationContext _context;
        public ReservationRepository()
        {
            _context = new ReservationContext("DB");
        }

        public void CreateReservation(Reservation theReservation)
        {
            _context.Reservations.Add(theReservation);
            _context.SaveChanges();
        }

        public void CancelReservation(Guid id, DateTime cancelDate, String reason)
        {
            var reservation = new Reservation()
            {
                ReservationId = id
            };
            _context.Reservations.Attach(reservation);

            reservation.CancellationDate = cancelDate;
            reservation.IsDeleted = true;
            reservation.Reason = reason;
            
            _context.SaveChanges();
        }

        public void DeleteReservation(Guid id)
        {
            var reservation = new Reservation()
            {
                ReservationId = id
            };
            _context.Reservations.Attach(reservation);
            _context.Reservations.Remove(reservation);

            _context.SaveChanges();
        }


        public IEnumerable<Reservation> GetAllReservations()
        {
            return _context.Reservations.AsEnumerable();
        }

        public Reservation GetReservationById(Guid id)
        {
            return _context.Reservations.SingleOrDefault(r => r.ReservationId == id);
        }
    }
}
