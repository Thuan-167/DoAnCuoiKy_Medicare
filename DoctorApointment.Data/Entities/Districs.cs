﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Data.Entities
{
    public class Districs
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int SortOrder { get; set; }

        //public List<Locations> Wards { get; set; }

    }
}
