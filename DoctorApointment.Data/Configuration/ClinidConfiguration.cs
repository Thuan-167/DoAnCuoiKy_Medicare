﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorApointment.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DoctorApointment.Data.Configuration
{
    public class ClinidConfiguration : IEntityTypeConfiguration<Clinics>
    {
        public void Configure(EntityTypeBuilder<Clinics> builder)
        {
            builder.ToTable("Clinics");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.No).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Address).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(int.MaxValue);
            builder.Property(x => x.Contact).HasMaxLength(int.MaxValue);
            builder.Property(x => x.Service).HasMaxLength(int.MaxValue);
            builder.Property(x => x.Note).HasMaxLength(int.MaxValue);
            builder.Property(x => x.ImgLogo).IsRequired().HasMaxLength(100);

            builder.HasOne(x => x.Locations).WithMany(x => x.Clinics).HasForeignKey(x => x.LocationId);
        }
    }
}
