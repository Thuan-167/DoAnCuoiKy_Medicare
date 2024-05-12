using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorApointment.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DoctorApointment.Data.Configuration
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointments>
    {
        public void Configure(EntityTypeBuilder<Appointments> builder)
        {
            builder.ToTable("Appointments");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.No).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Note).HasMaxLength(int.MaxValue);
            builder.Property(x => x.CancelReason).HasMaxLength(int.MaxValue);

            builder.HasOne(x => x.SchedulesSlots).WithMany(x => x.Appointments).HasForeignKey(x => x.SchedulesSlotId).OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(x => x.Patients).WithMany(x => x.Appointments).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.ClientCascade); ;
            builder.HasOne(x => x.Doctors).WithMany(x => x.Appointments).HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}

