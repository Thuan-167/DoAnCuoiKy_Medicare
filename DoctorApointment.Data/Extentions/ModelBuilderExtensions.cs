﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorApointment.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DoctorApointment.Data.Extentions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // any guid
            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var roleId1 = new Guid("2DD4EC71-5669-42D7-9CF9-BB17220C64C7");
            var roleId2 = new Guid("50FE257E-6475-41F0-93F7-F530D622362B");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            modelBuilder.Entity<AppRoles>().HasData(new AppRoles
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            }, new AppRoles
            {
                Id = roleId1,
                Name = "doctor",
                NormalizedName = "doctor",
                Description = "doctor role"
            }, new AppRoles
            {
                Id = roleId2,
                Name = "patient",
                NormalizedName = "patient",
                Description = "patient role"
            });
            var hasher = new PasswordHasher<AppUsers>();
            modelBuilder.Entity<AppUsers>().HasData(new AppUsers
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "thuanhuynh100@gmail.com",
                PhoneNumber = "0935219964",
                NormalizedEmail = "thuanhuynh100@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "thuan123@"),
                SecurityStamp = string.Empty,
                RoleId = roleId,
                Status = Enums.Status.Active
            });
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });
            modelBuilder.Entity<Specialities>().HasData(
               new Specialities() { Id = new Guid("8D04DCE4-969A-435D-BBA4-DF3F325983DC"), Title = "Tiêu hóa", Description = "Điều trị các bệnh về tiêu hoá", SortOrder = 1, No = "SP-22-001", IsDeleted = false, Img = "default", }

               );
            modelBuilder.Entity<Informations>().HasData(
              new Informations()
              {
                  Id = new Guid("B0603A9C-A60E-496F-B096-E1B18CAD69E0"),
                  Company = "Công ty TNHH DoctorMedio",
                  Email = "thuanhuynh100@gmail.com",
                  Hotline = "0935219964",
                  FullAddress = "89 Nam Cao, Phường Hòa Khánh Bắc, Quận Liên Chiều, Thành Phố Đà Nẵng",
                  IsDeleted = false,
                  Image = "default",
                  TimeWorking = "8:00-17:00 mỗi tuần",
                  AccountBank = "1900455685462",
                  AccountBankName = "Techcombank",
                  Content = "Nộp phí sử dụng dịch vụ bác sĩ.",
                  ServiceFee = 3500000
              }

              );
            modelBuilder.Entity<MainMenus>().HasData(
             new MainMenus() { Id = new Guid("8D04DCE4-969A-435D-BBA4-DF3F325983DC"), Controller = "Home", Action = "Index", SortOrder = 1, CratedAt = new DateTime(2000, 10, 02), IsDeleted = true, Image = "default", Name = "Trang Chủ", ParentId = new Guid(), Type = "MenuHeader", Title = "trang chủ", Description = "" }

             );
        }
    }
}
