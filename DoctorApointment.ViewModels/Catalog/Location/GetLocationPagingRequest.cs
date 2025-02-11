﻿using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.ViewModels.Catalog.Location
{
    public class GetLocationPagingRequest : PagingResultBase
    {
        public string? Type { get; set; }
        public string? Keyword { get; set; }
    }
}
