﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Data.Entities
{
    public class Medicines
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Image { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public List<MedicineDetailts> MedicineDetailts { get; set; }
    }
}
