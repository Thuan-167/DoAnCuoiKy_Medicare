﻿using DoctorApointment.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DoctorApointment.ViewModels.System.Users
{
    public class UserUpdatePatientRequest
    {
        public Guid Id { get; set; }

        [Display(Name = "Họ Tên")]
        public string Name { get; set; }

        [Display(Name = "Giới tính")]
        public Gender Gender { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Trạng thái")]
        public Status Status { get; set; }

        public string Address { get; set; }

        [Display(Name = "Hình ảnh")]
        public IFormFile? ThumbnailImage { get; set; }
        public string? img { get; set; }
    }
}
