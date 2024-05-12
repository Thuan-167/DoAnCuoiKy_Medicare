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
    public class HistoryActivesConfigration : IEntityTypeConfiguration<HistoryActives>
    {
        public void Configure(EntityTypeBuilder<HistoryActives> builder)
        {
            builder.ToTable("HistoryActives");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.User).HasMaxLength(100);
            builder.Property(x => x.Type).HasMaxLength(100);

        }
    }
}
