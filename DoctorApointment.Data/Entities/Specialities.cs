﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Data.Entities
{
    public class Specialities
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public string? Img { get; set; }
        public bool IsDeleted { get; set; }
        public string? No { get; set; }

        public List<ServicesSpecialities> ServicesSpecialities { get; set; }
    }
}
