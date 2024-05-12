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
    public class SchedulesDetailConfiguration : IEntityTypeConfiguration<SchedulesSlots>
    {
        public void Configure(EntityTypeBuilder<SchedulesSlots> builder)
        {
            builder.ToTable("SchedulesSlots");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Schedules).WithMany(x => x.schedulesSlots).HasForeignKey(x => x.ScheduleId);

        }
    }
}
