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
    public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.ToTable("MedicalRecords");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Diagnose).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Note).HasMaxLength(255);

            builder.HasOne(x => x.Appointments).WithOne(x => x.MedicalRecords).HasForeignKey<MedicalRecord>(x => x.AppointmentId);

        }
    }
}
