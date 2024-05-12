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
    public class SchedulesConfiguration : IEntityTypeConfiguration<Schedules>
    {
        public void Configure(EntityTypeBuilder<Schedules> builder)
        {
            builder.ToTable("Schedules");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Doctors).WithMany(x => x.Schedules).HasForeignKey(x => x.DoctorId);

        }
    }
}
