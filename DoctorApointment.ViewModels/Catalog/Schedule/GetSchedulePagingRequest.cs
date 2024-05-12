using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.ViewModels.Catalog.Schedule
{
    public class GetSchedulePagingRequest : PagingResultBase
    {
        public string? Keyword { get; set; }
    }
}
