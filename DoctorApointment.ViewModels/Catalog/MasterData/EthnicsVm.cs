﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DoctorApointment.ViewModels.Catalog.MasterData
{
    public class EthnicsVm
    {
        public Guid Id { get; set; }
        [Display(Name = "Tên dân tộc")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "vị tí sắp xếp")]
        public int SortOrder { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsDeleted { get; set; }

    }
}
