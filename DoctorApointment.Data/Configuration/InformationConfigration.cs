﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using DoctorApointment.Data.Entities;

namespace DoctorApointment.Data.Configuration
{
    public class InformationConfigration : IEntityTypeConfiguration<Informations>
    {
        public void Configure(EntityTypeBuilder<Informations> builder)
        {
            builder.ToTable("Informations");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.Property(x => x.FullAddress).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Company).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Image).IsRequired().HasMaxLength(100);
            builder.Property(x => x.TimeWorking).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Hotline).IsRequired().HasMaxLength(10);
            builder.Property(x => x.AccountBank).IsRequired().HasMaxLength(100);
            builder.Property(x => x.AccountBankName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Content).IsRequired().HasMaxLength(100);
            builder.Property(x => x.MapFrame).HasMaxLength(int.MaxValue);
        }
    }
}
