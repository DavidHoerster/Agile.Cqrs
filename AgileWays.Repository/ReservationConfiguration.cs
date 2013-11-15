using AgileWays.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Repository
{
    public class ReservationConfiguration : EntityTypeConfiguration<Reservation>
    {
        public ReservationConfiguration()
        {
            ToTable("Reservation");
            HasKey(r => r.ReservationId);
            Property(r => r.ReservationId)
                .HasColumnName("ReservationId");
        }
    }
}
