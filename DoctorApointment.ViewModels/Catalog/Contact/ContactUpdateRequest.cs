﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.ViewModels.Catalog.Contact
{
    public class ContactUpdateRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public string PhoneNumber { get; set; }
        public string YourMessage { get; set; }
    }
}
