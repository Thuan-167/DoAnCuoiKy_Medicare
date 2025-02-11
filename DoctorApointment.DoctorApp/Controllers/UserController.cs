﻿using DoctorApointment.Apilntegreation;
using DoctorApointment.Data.Enums;
using DoctorApointment.Utilities.Constants;
using DoctorApointment.ViewModels.Catalog.Speciality;
using DoctorApointment.ViewModels.System.Doctors;
using DoctorApointment.ViewModels.System.Models;
using DoctorApointment.ViewModels.System.Statistic;
using DoctorApointment.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DoctorApointment.DoctorApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly ILocationApiClient _locationApiClient;
        private readonly IStatisticApiClient _statisticApiClient;
        private readonly string NAMESAPACE = "DoctorApointment.DoctorApp.Controllers.User";

        public UserController(IUserApiClient userApiClient,
                        IConfiguration configuration, ILocationApiClient locationApiClient, IStatisticApiClient statisticApiClient)
        {
            _statisticApiClient = statisticApiClient;
            _userApiClient = userApiClient;
            _configuration = configuration;
            _locationApiClient = locationApiClient;
        }
        public async Task HistoryActive(HistoryActiveCreateRequest request)
        {
            var session = HttpContext.Session.GetString(SystemConstants.History);
            string? ServiceName = null;
            if (session != null)
            {
                var currentHistory = JsonConvert.DeserializeObject<HistoryActiveCreateRequest>(session);
                currentHistory.ToTime = DateTime.Now;
                ServiceName = currentHistory.ServiceName + request.MethodName;
                if (ServiceName != request.ServiceName + request.MethodName) await _statisticApiClient.AddActiveUser(currentHistory);

            }
            if (ServiceName == null || ServiceName != request.ServiceName + request.MethodName)
            {
                var history = new HistoryActiveCreateRequest()
                {
                    User = User.Identity.Name,
                    Usertemporary = User.Identity.Name,
                    Type = "doctor",
                    ServiceName = request.ServiceName,
                    MethodName = request.MethodName,
                    ExtraProperties = request.ExtraProperties,
                    Parameters = request.Parameters,
                    FromTime = DateTime.Now
                };

                HttpContext.Session.SetString(SystemConstants.History, JsonConvert.SerializeObject(history));
            }
        }
        public async Task<IActionResult> Index()
        {
            var result = await _userApiClient.GetByUserName(User.Identity.Name);
            var historyactive = new HistoryActiveCreateRequest()
            {
                ServiceName = NAMESAPACE + ".Index",
                MethodName = "Get",
                ExtraProperties = result.IsSuccessed ? "success" : "error",
                Parameters = "{}",
            };
            await HistoryActive(historyactive);
            if (result.IsSuccessed)
            {
                var user = result.Data;
                ViewBag.Img = user.DoctorVm.Img;
                ViewBag.Clinic = await _userApiClient.GetAllClinic(user.DoctorVm.GetClinic.Id);
                ViewBag.Imgs = user.DoctorVm.Galleries;
                ViewBag.SetChoices = JsonConvert.SerializeObject(await SeletectSpecialities(user.DoctorVm.GetSpecialities));
                ViewBag.Province = await _locationApiClient.GetAllProvince(new Guid());
                ViewBag.District = await _locationApiClient.CityGetAllDistrict(user.DoctorVm.Location.District.Id, user.DoctorVm.Location.District.Province.Id);
                ViewBag.SubDistrict = await _locationApiClient.GetAllSubDistrict(user.DoctorVm.Location.Id, user.DoctorVm.Location.District.Id);
                var updateRequest = new DocterRequestAll()
                {
                    Dob = user.DoctorVm.Dob,
                    FirstName = user.DoctorVm.FirstName,
                    LastName = user.DoctorVm.LastName,
                    Gender = user.DoctorVm.Gender,
                    Id = user.Id,
                    Address = user.DoctorVm.Address,
                    ClinicId = user.DoctorVm.GetClinic.Id,
                    Img = user.DoctorVm.Img,
                    Description = user.DoctorVm.Intro,
                    Slug = user.DoctorVm.Slug.Replace("-" + user.DoctorVm.No, ""),
                    Educations = user.DoctorVm.Educations,
                    Prefix = user.DoctorVm.Prefix,
                    MapUrl = user.DoctorVm.MapUrl,
                    Booking = user.DoctorVm.Booking,
                    BeforeBookingDay = user.DoctorVm.BeforeBookingDay,
                    Prizes = user.DoctorVm.Prizes,
                    ProvinceId = user.DoctorVm.Location.District.Province.Id,
                    DistrictId = user.DoctorVm.Location.District.Id,
                    SubDistrictId = user.DoctorVm.Location.Id,
                    Note = user.DoctorVm.Note,
                    TimeWorking = user.DoctorVm.TimeWorking,
                    GetGalleries = user.DoctorVm.Galleries,
                };
               
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Index(DoctorUpdateProfile request)
        {
            ViewBag.Img = request.Img;
            ViewBag.Imgs = request.GetGalleries==null?new List<GalleryVm>(): request.GetGalleries;
            //ViewBag.Clinic = await _userApiClient.GetAllClinic(request.ClinicId);
            ViewBag.Province = await _locationApiClient.GetAllProvince(new Guid());
            ViewBag.District = await _locationApiClient.CityGetAllDistrict(new Guid(), request.ProvinceId);
            ViewBag.SubDistrict = await _locationApiClient.GetAllSubDistrict(new Guid(), request.DistrictId);
            var getallspecialities = await _userApiClient.GetAllSpeciality(new Guid());
            var specialities = new List<GetSpecialityVm>();
            if(request.Specialities!= null) 
            {
                 foreach (var spe in request.Specialities)
            {
                var speciality = new GetSpecialityVm()
                {
                    Id = spe,
                    IsDeleted = false,
                    Title = getallspecialities.FirstOrDefault(x => x.Value == spe.ToString()).Text,
                };
                specialities.Add(speciality);
            }
            }
           
            ViewBag.SetChoices = JsonConvert.SerializeObject(await SeletectSpecialities(specialities));
            var updateRequest = new DocterRequestAll()
            {
                Dob = request.Dob,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                Id = request.Id,
                Address = request.Address,
                //ClinicId = request.ClinicId,
                Img = request.Img,
                Description = request.Description,
                Educations = request.Educations,
                Prefix = request.Prefix,
                MapUrl = request.MapUrl,
                Booking = request.Booking,
                BeforeBookingDay = request.BeforeBookingDay,
                Prizes = request.Prizes,
                ProvinceId = request.ProvinceId,
                DistrictId = request.DistrictId,
                SubDistrictId = request.SubDistrictId,
                Note = request.Note,
                TimeWorking = request.TimeWorking,
                GetGalleries = request.GetGalleries == null ? new List<GalleryVm>() : request.GetGalleries
        };
            if (!ModelState.IsValid)
                return View(updateRequest);
            var result = await _userApiClient.DoctorUpdateProfile(request);
            var historyactive = new HistoryActiveCreateRequest()
            {
                ServiceName = NAMESAPACE + ".Index",
                MethodName = "Get",
                ExtraProperties = result.IsSuccessed ? "success" : "error",
                Parameters = JsonConvert.SerializeObject(request),
            };
            await HistoryActive(historyactive);
            if (result.IsSuccessed)
            {
                TempData["AlertMessage"] = "Thay đổi thông tin thành công";
                TempData["AlertType"] = "success";
                TempData["AlertId"] = "successToast";
                return RedirectToAction("Index");
            }
          
            if (result.ValidationErrors[0] == "warning")
            {
                TempData["AlertMessage"] = result.ValidationErrors[1];
                TempData["AlertType"] = "warning";
                TempData["AlertId"] = "warningToast";
                return RedirectToAction("Index");
            }
            TempData["AlertMessage"] = result.ValidationErrors[1];
            TempData["AlertType"] = "danger";
            TempData["AlertId"] = "dangerToast";
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var result = await _userApiClient.GetByUserName(User.Identity.Name);
            var historyactive = new HistoryActiveCreateRequest()
            {
                ServiceName = NAMESAPACE + ".Update",
                MethodName = "Get",
                ExtraProperties = result.IsSuccessed ? "success" : "error",
                Parameters = "{user: " + User.Identity.Name + "}",
            };
            await HistoryActive(historyactive);
            if (result.IsSuccessed)
            {
                var user = result.Data;
                ViewBag.Img = user.DoctorVm.Img;
                ViewBag.Clinic = await _userApiClient.GetAllClinic(user.DoctorVm.GetClinic.Id);
                ViewBag.Imgs = user.DoctorVm.Galleries;
                ViewBag.SetChoices = JsonConvert.SerializeObject(await SeletectSpecialities(user.DoctorVm.GetSpecialities));
                ViewBag.Province = await _locationApiClient.GetAllProvince(new Guid());
                ViewBag.District = await _locationApiClient.CityGetAllDistrict(user.DoctorVm.Location.District.Id, user.DoctorVm.Location.District.Province.Id);
                ViewBag.SubDistrict = await _locationApiClient.GetAllSubDistrict(user.DoctorVm.Location.Id, user.DoctorVm.Location.District.Id);
                var updateRequest = new DoctorUpdateRequest()
                {
                    Dob = user.DoctorVm.Dob,
                    FirstName = user.DoctorVm.FirstName,
                    LastName = user.DoctorVm.LastName,
                    Gender = user.DoctorVm.Gender,
                    Id = user.Id,
                    Address = user.DoctorVm.Address,
                    ClinicId = user.DoctorVm.GetClinic.Id,
                    //img = user.DoctorVm.Img,
                    Description = user.DoctorVm.Intro,
                    Educations = user.DoctorVm.Educations,
                    Prefix = user.DoctorVm.Prefix,
                    MapUrl = user.DoctorVm.MapUrl,
                    Booking = user.DoctorVm.Booking,
                    Prizes = user.DoctorVm.Prizes,
                    ProvinceId = user.DoctorVm.Location.District.Province.Id,
                    DistrictId = user.DoctorVm.Location.District.Id,
                    SubDistrictId = user.DoctorVm.Location.Id,
                    Note = user.DoctorVm.Note,
                    TimeWorking = user.DoctorVm.TimeWorking,
                    GetGalleries = user.DoctorVm.Galleries
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        public async Task<List<SetChoicesVm>> SeletectSpecialities(List<GetSpecialityVm> specialities)
        {
            var chose = await _userApiClient.GetAllSpeciality(new Guid());
            var rs = new List<SetChoicesVm>();
            foreach (var spe in specialities)
            {

                var chose_spe = new SetChoicesVm()
                {
                    selected = true,
                    label = spe.Title,
                    value = spe.Id.ToString()
                };
                rs.Add(chose_spe);
                chose = chose.Where(x => x.Value != spe.Id.ToString()).ToList();
            }
            foreach (var spe in chose)
            {
                var chose_spe = new SetChoicesVm()
                {
                    selected = false,
                    label = spe.Text,
                    value = spe.Value
                };
                rs.Add(chose_spe);
            }
            return rs;
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] DoctorUpdateRequest request)
        {
            ViewBag.Imgs = request.GetGalleries;
            ViewBag.Clinic = await _userApiClient.GetAllClinic(request.ClinicId);
            ViewBag.Province = await _locationApiClient.GetAllProvince(new Guid());
            ViewBag.District = await _locationApiClient.CityGetAllDistrict(new Guid(), request.ProvinceId);
            ViewBag.SubDistrict = await _locationApiClient.GetAllSubDistrict(new Guid(), request.DistrictId);
            var getallspecialities = await _userApiClient.GetAllSpeciality(new Guid());
            var specialities = new List<GetSpecialityVm>();
            if (request.Specialities != null) 
            {
                foreach (var spe in request.Specialities)
                {
                    var speciality = new GetSpecialityVm()
                    {
                        Id = spe,
                        IsDeleted = false,
                        Title = getallspecialities.FirstOrDefault(x => x.Value == spe.ToString()).Text,
                    };
                    specialities.Add(speciality);
                }
            }        

            ViewBag.SetChoices = JsonConvert.SerializeObject(await SeletectSpecialities(specialities)); ;
            if (!ModelState.IsValid)
                return View(request);

            var result = await _userApiClient.DoctorUpdateRequest(request.Id, request);
            var historyactive = new HistoryActiveCreateRequest()
            {
                ServiceName = NAMESAPACE + ".Update",
                MethodName = "Post",
                ExtraProperties = result.IsSuccessed ? "success" : "error",
                Parameters = JsonConvert.SerializeObject(request),
            };
            await HistoryActive(historyactive);
            if (result.IsSuccessed)
                {
                    TempData["AlertMessage"] = "Thay đổi thông tin thành công";
                    TempData["AlertType"] = "alert-success";
                    return RedirectToAction("DetailtDoctor", new { userName = User.Identity.Name});
                }

               
                return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> GetSubDistrict(Guid DistrictId)
        {
            if (!string.IsNullOrWhiteSpace(DistrictId.ToString()))
            {
                var district = await _locationApiClient.GetAllSubDistrict(null, DistrictId);
                return Json(district);
            }
            return null;
        }
        [HttpGet]
        public async Task<IActionResult> GetDistrict(Guid ProvinceId)
        {
            if (!string.IsNullOrWhiteSpace(ProvinceId.ToString()))
            {
                var district = await _locationApiClient.CityGetAllDistrict(null, ProvinceId);
                return Json(district);
            }
            return null;
        }
        public List<SelectListItem> SeletectGender(string id)
        {
            List<SelectListItem> gender = new List<SelectListItem>()
            {
                new SelectListItem(text: "Nam", value: "0"),
                new SelectListItem(text: "Nữ", value: "1")
            };
            var rs = gender.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value,
                Selected = id == x.Value
            }).ToList();
            return rs;
        }
       
        [HttpGet]
        public async Task<IActionResult> DetailtDoctor()
        {
            var result = await _userApiClient.GetByUserName(User.Identity.Name);
            if (result.IsSuccessed)
            {
                var user = result.Data;
                ViewBag.Img = user.DoctorVm.Img;
                ViewBag.Gender = user.Gender == Gender.Male ? "Nam" : "Nữ";
                ViewBag.Dob = user.Dob.ToShortDateString();
                ViewBag.Status = user.Status == Status.NotActivate ? "Ngừng hoạt động" : user.Status == Status.Active ? "Hoạt động" : "không hoạt động";
               
                return View(user);
            }
            return RedirectToAction("Error", "Home");
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.ChangePassword(request);
            var historyactive = new HistoryActiveCreateRequest()
            {
                ServiceName = NAMESAPACE + ".ChangePassword",
                MethodName = "Post",
                ExtraProperties = result.IsSuccessed ? "success" : "error",
                Parameters = JsonConvert.SerializeObject(request),
            };
            await HistoryActive(historyactive);
            if (result.IsSuccessed)
            {
                TempData["AlertMessage"] = "Thay đổi thông tin thành công";
                TempData["AlertType"] = "success";
                TempData["AlertId"] = "successToast";
                return RedirectToAction("Index");
            }
            if (result.ValidationErrors[0] == "warning")
            {
                TempData["AlertMessage"] = result.ValidationErrors[1];
                TempData["AlertType"] = "warning";
                TempData["AlertId"] = "warningToast";
                return RedirectToAction("Index");
            }
            TempData["AlertMessage"] = result.ValidationErrors[1];
            TempData["AlertType"] = "danger";
            TempData["AlertId"] = "dangerToast";
            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> IsBooking(Guid doctorId)
        {
            var result = await _userApiClient.IsBooking(doctorId);
            return Json(new { response = result });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteImg(Guid imgId)
        {
            var result = await _userApiClient.DeleteImg(imgId);
            return Json(new { response = result });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAllImg(Guid doctorId)
        {
            var result = await _userApiClient.DeleteAllImg(doctorId);
            return Json(new { response = result });
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }
    }
}
