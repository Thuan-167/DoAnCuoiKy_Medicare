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
    public class SpecialityConfiguration : IEntityTypeConfiguration<Specialities>
    {
        public void Configure(EntityTypeBuilder<Specialities> builder)
        {
            builder.ToTable("Specialities");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Img).IsRequired().HasMaxLength(255);
            builder.Property(x => x.No).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(255);
        }
    }
}
