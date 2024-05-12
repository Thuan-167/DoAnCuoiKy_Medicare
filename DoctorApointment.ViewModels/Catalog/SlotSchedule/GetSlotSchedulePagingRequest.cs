using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.ViewModels.Catalog.SlotSchedule
{
    public class GetSlotSchedulePagingRequest : PagingResultBase
    {
        public string? Keyword { get; set; }
    }
}
