﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Data.Entities
{
    public class Rates
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Rating { get; set; }
        public Guid AppointmentId { get; set; }
        //public Status Status { get; set; }
        public Appointments Appointments { get; set; }
    }
}
