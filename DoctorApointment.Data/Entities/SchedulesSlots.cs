using DoctorApointment.Data.Entities;
using DoctorApointment.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Data.Entities
{
    public class SchedulesSlots
    {
        public Guid Id { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBooked { get; set; }
        public Schedules Schedules { get; set; }
        public Guid ScheduleId { get; set; }
        public List<Appointments> Appointments { get; set; }
    }
}
