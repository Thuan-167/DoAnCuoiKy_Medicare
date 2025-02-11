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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUsers>
    {
        public void Configure(EntityTypeBuilder<AppUsers> builder)
        {
            builder.ToTable("AppUsers");

            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(11);

            builder.HasOne(x => x.AppRoles).WithMany(x => x.Users).HasForeignKey(x => x.RoleId);
        }
    }
}
