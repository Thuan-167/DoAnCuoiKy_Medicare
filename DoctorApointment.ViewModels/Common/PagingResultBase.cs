﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.ViewModels.Common
{
    public class PagingResultBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
