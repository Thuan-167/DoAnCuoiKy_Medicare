﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Data.Entities
{
    public class ImagePost
    {
        public Guid Id { get; set; }
        public string Img { get; set; }
        public int SortOrder { get; set; }
    }
}
