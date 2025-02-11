﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Data.Entities
{
    public class Informations
    {
        public Guid Id { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public string Image { get; set; }
        public string Hotline { get; set; }
        public string TimeWorking { get; set; }
        public string FullAddress { get; set; }
        public string AccountBank { get; set; }
        public string AccountBankName { get; set; }
        public decimal ServiceFee { get; set; }
        public string Content { get; set; }
        public string? MapFrame { get; set; }
    }
}
