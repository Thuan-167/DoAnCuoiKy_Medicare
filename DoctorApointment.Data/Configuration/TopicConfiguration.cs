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
    public class TopicConfiguration : IEntityTypeConfiguration<Topics>
    {
        public void Configure(EntityTypeBuilder<Topics> builder)
        {
            builder.ToTable("Topics");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Image).HasMaxLength(100);
            builder.Property(x => x.Titile).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(int.MaxValue);

        }
    }
}
