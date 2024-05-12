using DoctorApointment.Apilntegreation;
using Microsoft.AspNetCore.Mvc;

namespace DoctorApointment.WebApp.Controllers.Components
{
    public class UserInfoViewComponent : ViewComponent
    {
        private readonly IDoctorApiClient _doctorApiClient;
        public UserInfoViewComponent(IDoctorApiClient doctorApiClient)
        {
            _doctorApiClient = doctorApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.Name == null) return View();
            var patients = (await _doctorApiClient.GetPatientProfile(User.Identity.Name)).Data;
            var patient = patients.FirstOrDefault(x => x.IsPrimary == true);
            ViewBag.PatientName = patient.Name;
            return View();
        }

    }
}

