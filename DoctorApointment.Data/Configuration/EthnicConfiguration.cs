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
    public class EthnicConfiguration : IEntityTypeConfiguration<Ethnics>
    {
        public void Configure(EntityTypeBuilder<Ethnics> builder)
        {
            builder.ToTable("Ethnics");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

        }
    }
}
