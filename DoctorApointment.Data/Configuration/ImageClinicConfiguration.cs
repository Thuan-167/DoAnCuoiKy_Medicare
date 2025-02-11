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
    public class ImageClinicConfiguration : IEntityTypeConfiguration<ImageClinics>
    {
        public void Configure(EntityTypeBuilder<ImageClinics> builder)
        {
            builder.ToTable("ImageClinics");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Img).IsRequired().HasMaxLength(100);


            builder.HasOne(x => x.Clinics).WithMany(x => x.ImageClinics).HasForeignKey(x => x.ClinicId);

        }
    }
}
