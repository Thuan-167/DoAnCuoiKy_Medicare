﻿using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.ViewModels.Catalog.Post
{
    public class GetPostPagingRequest : PagingResultBase
    {
        public string? Keyword { get; set; }
        public Guid? TopicId { get; set; }
        public string? Usename { get; set; }
    }
}
