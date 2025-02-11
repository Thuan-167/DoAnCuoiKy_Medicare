﻿using DoctorApointment.Apilntegreation;
using DoctorApointment.Data.Enums;
using DoctorApointment.ViewModels.Catalog.Speciality;
using DoctorApointment.ViewModels.System.Models;
using DoctorApointment.ViewModels.System.Statistic;
using DoctorApointment.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DoctorApointment.AdminApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IStatisticApiClient _statisticApiClient;
        private readonly IConfiguration _configuration;
        private readonly ILocationApiClient _locationApiClient;

        public UserController(IUserApiClient userApiClient,
            IConfiguration configuration, ILocationApiClient locationApiClient,
            IStatisticApiClient statisticApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _locationApiClient = locationApiClient;
            _statisticApiClient = statisticApiClient;
        }

        public async Task<IActionResult> Index(string keyword, string day, string month, string year, string check, string rolename, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                RoleName = rolename
            };
            var data = await _userApiClient.GetUsersPagings(request);
            var requeststatictic = new GetHistoryActivePagingRequest()
            {
                Keyword = keyword,
                day = day,
                month = month,
                year = year,
                role = rolename
            };
            ViewBag.Statitic = JsonConvert.SerializeObject(await Statistic(requeststatictic,day,month,year,check));
           
            ViewBag.Keyword = keyword;
            ViewBag.Day = requeststatictic.day == null ? DateTime.Now.ToString("dd") : requeststatictic.day;
            ViewBag.Month = requeststatictic.month == null ? DateTime.Now.ToString("MM") : requeststatictic.month;
            ViewBag.Year = requeststatictic.year;
            ViewBag.Role = requeststatictic.role;
            ViewBag.Check = check;
            ViewBag.rolename = SeletectRole(requeststatictic.role==null?"": requeststatictic.role);
            ViewBag.Days = SeletectDay(requeststatictic.day == null ? DateTime.Now.ToString("dd") : requeststatictic.day);
            ViewBag.Months = SeletectMonth(requeststatictic.month == null ? DateTime.Now.ToString("MM") : requeststatictic.month);
            ViewBag.Years = SeletectYear(requeststatictic.year);
           
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data.Data);
        }
        public async Task<List<StatisticActive>> Statistic(GetHistoryActivePagingRequest requeststatictic, string? day, string? month, string? year, string? check)
        {
            switch (check)
            {
                case "year" or null:
                    requeststatictic.year = year == null ? DateTime.Now.ToString("yyyy") : year;
                    requeststatictic.month = null;
                    requeststatictic.day = null;
                    return await _statisticApiClient.GetServiceFeeStatiticYear(requeststatictic);

                case "month":
                    requeststatictic.day = null;
                    requeststatictic.month = month == null ? DateTime.Now.ToString("MM") : month;
                    requeststatictic.year = year == null ? DateTime.Now.ToString("yyyy") : year;
                    return await _statisticApiClient.GetServiceFeeStatiticMonth(requeststatictic);

                default:
                    requeststatictic.day = day == null ? DateTime.Now.ToString("dd") : day;
                    requeststatictic.month = month == null ? DateTime.Now.ToString("MM") : month;
                    requeststatictic.year = year == null ? DateTime.Now.ToString("yyyy") : year;
                    return await _statisticApiClient.GetServiceFeeStatiticDay(requeststatictic);
            }
        }

        public List<SelectListItem> SeletectRole(string? str)
        {
            List<SelectListItem> role = new List<SelectListItem>()
            {
                new SelectListItem(text: "Bác sĩ", value: "doctor"),
                new SelectListItem(text: "Bệnh nhân", value: "patient"),
                new SelectListItem(text: "Tất cả", value: ""),
                new SelectListItem(text: "Quản trị", value: "admin")
            };
           
            var rs = role.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value,
                Selected = str == x.Value
            }).ToList();
            return rs;
        }
       
        public async Task<IActionResult> ListRole()
        {
            var data = await _userApiClient.GetAllRole();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Clinic = await _userApiClient.GetAllClinic(new Guid());
            ViewBag.SetChoices = JsonConvert.SerializeObject(await SeletectSpecialities(new List<GetSpecialityVm>()));
            ViewBag.Province = await _locationApiClient.GetAllProvince(new Guid());
            ViewBag.District = new List<SelectListItem>();
            ViewBag.SubDistrict = new List<SelectListItem>();

            return View();
        }

        [HttpPost]
        //[Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(ManageRegisterRequest request)
        {
            ViewBag.Clinic = await _userApiClient.GetAllClinic(request.ClinicId);
            
            ViewBag.Province = await _locationApiClient.GetAllProvince(request.ProvinceId);
            ViewBag.District = await _locationApiClient.CityGetAllDistrict(request.DistrictId, request.ProvinceId==null?new Guid():request.ProvinceId);
            ViewBag.SubDistrict = await _locationApiClient.GetAllSubDistrict(request.SubDistrictId,request.DistrictId == null ? new Guid() : request.DistrictId);
            var getallspecialities = await _userApiClient.GetAllSpeciality(new Guid());
            var specialities = new List<GetSpecialityVm>();
            if (request.SpecialityId != null)
            {
                foreach (var spe in request.SpecialityId)
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
            if (!ModelState.IsValid)
                return View(request);
            
            var result = await _userApiClient.RegisterDocter(request);

            if (result.IsSuccessed)
            {
                TempData["AlertMessage"] = "Thêm mới người dùng thành công";
                TempData["AlertType"] = "alert-success";
                return RedirectToAction("Index");
            }
            TempData["AlertMessage"] = result.Message;
            TempData["AlertType"] = "alert-warning";
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
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.Data;
                ViewBag.Status = SeletectStatus(user.Status);
                ViewBag.Clinic = await _userApiClient.GetAllClinic(user.DoctorVm.GetClinic.Id);
                ViewBag.SetChoices = JsonConvert.SerializeObject(await SeletectSpecialities(user.DoctorVm.GetSpecialities));
                ViewBag.Province = await _locationApiClient.GetAllProvince(new Guid());
                ViewBag.District = await _locationApiClient.CityGetAllDistrict(user.DoctorVm.Location.District.Id, user.DoctorVm.Location.District.Province.Id);
                ViewBag.SubDistrict = await _locationApiClient.GetAllSubDistrict(user.DoctorVm.Location.Id, user.DoctorVm.Location.District.Id);
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.DoctorVm.Dob,
                    FirstName = user.DoctorVm.FirstName,
                    LastName = user.DoctorVm.LastName,
                    Gender = user.DoctorVm.Gender,
                    Id = id,
                    Address = user.DoctorVm.Address,
                    Status = user.Status,
                    ClinicId = user.DoctorVm.GetClinic.Id,
                    Description = user.DoctorVm.Intro,
                    Slug = user.DoctorVm.Slug.Replace("-"+user.DoctorVm.No,""),
                    Educations = user.DoctorVm.Educations,
                    Prefix = user.DoctorVm.Prefix,
                    MapUrl = user.DoctorVm.MapUrl,
                    Booking = user.DoctorVm.Booking,
                    IsPrimary = user.DoctorVm.IsPrimary,
                    Prizes = user.DoctorVm.Prizes,
                    ProvinceId = user.DoctorVm.Location.District.Province.Id,
                    DistrictId = user.DoctorVm.Location.District.Id,
                    SubDistrictId = user.DoctorVm.Location.Id,
                    Note = user.DoctorVm.Note,
                    TimeWorking = user.DoctorVm.TimeWorking,
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
            foreach(var spe in chose)
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
        public async Task<IActionResult> Update( UserUpdateRequest request)
        {
            ViewBag.Status = SeletectStatus(request.Status);
            ViewBag.Clinic = await _userApiClient.GetAllClinic(request.ClinicId);
            ViewBag.Province = await _locationApiClient.GetAllProvince(new Guid());
            ViewBag.District = await _locationApiClient.CityGetAllDistrict(new Guid(), request.ProvinceId);
            ViewBag.SubDistrict = await _locationApiClient.GetAllSubDistrict(new Guid(), request.DistrictId);
            var getallspecialities = await _userApiClient.GetAllSpeciality(new Guid());
            var specialities = new List<GetSpecialityVm>();
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
            
            ViewBag.SetChoices = JsonConvert.SerializeObject(await SeletectSpecialities(specialities));
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.UpdateDoctor(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["AlertMessage"] = "Thay đổi thông tin người dùng thành công";
                TempData["AlertType"] = "alert-success";
                return RedirectToAction("Index");
            }

            
            return View(request);
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
        public async Task<IActionResult> UpdateAdmin(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            //var doctor = await _userApiClient.Get
            if (result.IsSuccessed)
            {
                var user = result.Data;
                ViewBag.Gender = SeletectGender(user.Gender.ToString());
                ViewBag.Status = SeletectStatus(user.Status);
                var updateRequest = new UserUpdateAdminRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gender,
                    Id = id,
                    Status = user.Status 
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdmin(UserUpdateAdminRequest request)
        {
            ViewBag.Gender = SeletectGender(request.Gender.ToString());
            ViewBag.Status = SeletectStatus(request.Status);
            if (!ModelState.IsValid)
                return View();
            
            var result = await _userApiClient.UpdateAdmin(request);
            if (result.IsSuccessed)
            {
                TempData["AlertMessage"] = "Thay đổi thông tin người dùng thành công";
                TempData["AlertType"] = "alert-success";
                return RedirectToAction("Index");
            }

          
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> UpdatePatient(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            
            //var doctor = await _userApiClient.Get
           
            if (result.IsSuccessed)
            {
                var user = result.Data;
                ViewBag.Gender = SeletectGender(user.Gender.ToString());
                ViewBag.Status = SeletectStatus(user.Status);
                ViewBag.Img = user.PatientVm.Img;
                var updateRequest = new UserUpdatePatientRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gender,
                    Id = id,
                    Address = user.PatientVm.Address,
                    Status = user.Status 
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePatient([FromForm] UserUpdatePatientRequest request)
        {
            ViewBag.Img = request.img;
            ViewBag.Gender = SeletectGender(request.Gender.ToString());
            ViewBag.Status = SeletectStatus(request.Status);
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.UpdatePatient(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["AlertMessage"] = "Thay đổi thông tin người dùng thành công";
                TempData["AlertType"] = "alert-success";
                return RedirectToAction("Index");
            }

          
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> DetailtDoctor(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.Data;
                ViewBag.Img = user.DoctorVm.Img;
                ViewBag.Gender = user.DoctorVm.Gender == Gender.Male ? "Nam" : "Nữ";
                ViewBag.Dob = user.DoctorVm.Dob.ToShortDateString();
                ViewBag.Status = user.Status == Status.NotActivate ? "Ngừng hoạt động" : user.Status == Status.Active ? "Hoạt động": "không hoạt động";
                ViewBag.Specialities = user.DoctorVm.GetSpecialities;
                var updateRequest = new UserVm()
                {
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Id = id,
                    Address = user.Address,
                    Img = user.Img,
                    UserName = user.UserName,
                    GetRole = user.GetRole,
                    DoctorVm = user.DoctorVm,
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        /*public List<SelectListItem> SeletectStatus(Status status)
        {
            List<SelectListItem> lstatus = new List<SelectListItem>()
            {
                new SelectListItem(text: "Ngừng hoạt động", value: Status.NotActivate.ToString()),
                new SelectListItem(text: "Hoạt động", value: Status.Active.ToString()),
                new SelectListItem(text: "Không hoạt động", value: Status.InActive.ToString())
            };
            var rs = lstatus.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value,
                Selected = status.ToString() == x.Value
            }).ToList();
            return rs;
        }*/
        [HttpGet]
        public async Task<IActionResult> DetailtPatient(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.Data;
                ViewBag.Img = user.PatientVm.Img;
                ViewBag.Gender = user.PatientVm.Gender == Gender.Male ? "Nam" : "Nữ";
                ViewBag.Dob = user.PatientVm.Dob.ToShortDateString();
                ViewBag.Status = user.Status == Status.NotActivate ? "Ngừng hoạt động" : user.Status == Status.Active ? "Hoạt động" : "không hoạt động";
                var updateRequest = new UserVm()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Id = id,
                    Address = user.Address,
                    Img = user.Img,
                    UserName = user.UserName,
                    Status = user.Status //== Status.Active ? true : false
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Detailt(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.Data;
                ViewBag.Gender = user.Gender == Gender.Male ? "Nam" : "Nữ";
                ViewBag.Dob = user.Dob.ToShortDateString();
                ViewBag.Status = user.Status == Status.NotActivate ? "Ngừng hoạt động" : user.Status == Status.Active ? "Hoạt động" : "không hoạt động";
                var updateRequest = new UserVm()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Id = id,
                    Address = user.Address,
                    Img = user.Img,
                    UserName = user.UserName,
                    Status = user.Status //== Status.Active ? true : false
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid idUser)
        {
            var result = await _userApiClient.Delete(idUser);
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
