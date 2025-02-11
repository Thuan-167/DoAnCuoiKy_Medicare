﻿using DoctorApointment.Data.Entities;
using DoctorApointment.Data.Enums;
using DoctorApointment.ViewModels.Catalog.MedicalRecords;
using DoctorApointment.ViewModels.Catalog.Schedule;
using DoctorApointment.ViewModels.Catalog.SlotSchedule;
using DoctorApointment.ViewModels.System.Doctors;
using DoctorApointment.ViewModels.System.Patient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DoctorApointment.ViewModels.Catalog.Appointment
{
    public class AppointmentVm
    {
        public Guid Id { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "STT")]
        public int Stt { get; set; }
        [Display(Name = "Mã lịch hẹn")]
        public string No { get; set; }
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        public bool IsDoctor { get; set; }
        [Display(Name = "Trạng thái")]
        public StatusAppointment Status { get; set; }
        [Display(Name = "Lịch khám")]
        public ScheduleVm Schedule { get; set; }
        [Display(Name = "Chi tiết lịch khám")]
        public SlotScheduleVm SlotSchedule { get; set; }
        [Display(Name = "Bệnh nhân")]
        public PatientVm Patient { get; set; }
        [Display(Name = "Bác sĩ")]
        public DoctorVm Doctor { get; set; }
        [Display(Name = "Kết quả khám")]
        public MedicalRecordVm MedicalRecord { get; set; }
        [Display(Name = "Đánh giá")]
        public RateVm Rate { get; set; }
        [Display(Name = "Lý do hủy")]
        public string CancelReason { get; set; }
    }
}
