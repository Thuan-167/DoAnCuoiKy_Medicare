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
    public class GalleryConfiguration : IEntityTypeConfiguration<Galleries>
    {
        public void Configure(EntityTypeBuilder<Galleries> builder)
        {
            builder.ToTable("Galleries");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Img).IsRequired().HasMaxLength(100);

            builder.HasOne(x => x.Doctors).WithMany(x => x.Galleries).HasForeignKey(x => x.DoctorId);

        }
    }
}
