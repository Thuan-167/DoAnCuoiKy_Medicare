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
    public class MedicineConfigration : IEntityTypeConfiguration<Medicines>
    {
        public void Configure(EntityTypeBuilder<Medicines> builder)
        {
            builder.ToTable("Medicines");
            
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).HasMaxLength(int.MaxValue);
            builder.Property(x => x.Image).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Unit).IsRequired().HasMaxLength(50);
        }
    }
}
