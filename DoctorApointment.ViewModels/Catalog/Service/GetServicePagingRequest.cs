﻿using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.ViewModels.Catalog.Service
{
    public class GetServicePagingRequest : PagingResultBase
    {
        public string? Keyword { get; set; }
        public string? UserName { get; set; }
    }
}
