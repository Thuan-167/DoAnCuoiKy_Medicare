﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.ViewModels.System.Users
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
        public string? Role { get; set; }
    }
}
