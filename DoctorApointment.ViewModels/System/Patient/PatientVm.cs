﻿using DoctorApointment.Data.Enums;
using DoctorApointment.ViewModels.Catalog.Appointment;
using DoctorApointment.ViewModels.Catalog.Location;
using DoctorApointment.ViewModels.Catalog.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.ViewModels.System.Patient
{
    public class PatientVm
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Text { get; set; }
        public string Img { get; set; }
        public string No { get; set; }
        public Gender Gender { get; set; }
        public string GenderText { get; set; }
        public string DobText { get; set; }
        public DateTime Dob { get; set; }
        public bool IsPrimary { get; set; }
        public string RelativeName { get; set; }
        public Guid RelativeRelationshipId { get; set; }
        public string RelativePhone { get; set; }
        public string Identitycard { get; set; }
        public EthnicVm Ethnics { get; set; }
        public LocationVm Location { get; set; }

        public List<AppointmentVm> Appointments { get; set; }
        public List<MedicalRecordVm> MedicalRecords { get; set; }
        public Guid EthnicId { get; set; }
        public string FullAddress { get; set; }
        public string RelativeEmail { get; set; }
        public int CountBooking { get; set; }
    }
}
