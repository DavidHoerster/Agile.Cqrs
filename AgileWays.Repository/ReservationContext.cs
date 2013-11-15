using AgileWays.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Repository
{
    public class ReservationContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }

        public ReservationContext(String connString) : base(connString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ReservationConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
