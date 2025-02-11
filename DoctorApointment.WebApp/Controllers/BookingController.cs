﻿using DoctorApointment.Apilntegreation;
using DoctorApointment.Utilities.Constants;
using DoctorApointment.ViewModels.Catalog.Appointment;
using DoctorApointment.ViewModels.Catalog.Schedule;
using DoctorApointment.ViewModels.System.Patient;
using DoctorApointment.ViewModels.System.Statistic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using DoctorApointment.ViewModels.Catalog.Clinic;

namespace DoctorApointment.WebApp.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserApiClient _userApiClient;
        private readonly IDoctorApiClient _doctorApiClient;
        private readonly ISpecialityApiClient _specialityApiClient;
        private readonly IScheduleApiClient _scheduleApiClient;
        private readonly IAppointmentApiClient _appointmentApiClient;
        private readonly IClinicApiClient _clinicalApiClient;
        private readonly ILocationApiClient _locationApiClient;
        private readonly IStatisticApiClient _statisticApiClient;
        private readonly string NAMESAPACE = "DoctorApointment.WebApp.Controllers.Home";
        public BookingController(ILogger<HomeController> logger, IUserApiClient userApiClient, IDoctorApiClient doctorApiClient,
            ISpecialityApiClient specialityApiClient, IScheduleApiClient scheduleApiClient, IAppointmentApiClient appointmentApiClient,
            IClinicApiClient clinicApiClient, ILocationApiClient locationApiClient, IStatisticApiClient statisticApiClient)
        {
            _logger = logger;
            _userApiClient = userApiClient;
            _doctorApiClient = doctorApiClient;
            _specialityApiClient = specialityApiClient;
            _scheduleApiClient = scheduleApiClient;
            _appointmentApiClient = appointmentApiClient;
            _clinicalApiClient = clinicApiClient;
            _locationApiClient = locationApiClient;
            _statisticApiClient = statisticApiClient;
        }
        public async Task HistoryActive(HistoryActiveCreateRequest request)
        {
            var session = HttpContext.Session.GetString(SystemConstants.History);
            string? usertemporary = null;
            string? user = null;
            string? ServiceName = null;
            if (session != null)
            {
                var currentHistory = JsonConvert.DeserializeObject<HistoryActiveCreateRequest>(session);
                currentHistory.ToTime = DateTime.Now;
                ServiceName = currentHistory.ServiceName + request.MethodName;
                if (ServiceName != request.ServiceName + request.MethodName) await _statisticApiClient.AddActiveUser(currentHistory);
                usertemporary = currentHistory.Usertemporary;
                user = currentHistory.User;
            }
            if (ServiceName == null || ServiceName != request.ServiceName + request.MethodName)
            {
                var history = new HistoryActiveCreateRequest()
                {
                    User = User.Identity.Name == null ? user : User.Identity.Name,
                    Usertemporary = (usertemporary == null && User.Identity.Name == null) ? ("patient" + new Random().Next(10000000, 99999999) + new Random().Next(10000000, 99999999)) : (usertemporary == null ? User.Identity.Name : usertemporary),
                    Type = user == null ? "passersby" : "patient",
                    ServiceName = request.ServiceName,
                    MethodName = request.MethodName,
                    ExtraProperties = request.ExtraProperties,
                    Parameters = request.Parameters,
                    FromTime = DateTime.Now
                };

                HttpContext.Session.SetString(SystemConstants.History, JsonConvert.SerializeObject(history));
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BookingDoctorSetDate()
        {
            return View();
        }
        public async Task<IActionResult> BookingDoctorSetPatient(Guid doctorid, Guid scheduleid)
        {
            ViewBag.Ethnics = await _userApiClient.GetAllEthnicGroup();
            ViewBag.Province = await _locationApiClient.GetAllProvince(new Guid());
            ViewBag.District = new List<SelectListItem>();
            ViewBag.SubDistrict = new List<SelectListItem>();
            ViewBag.Patient = (await _doctorApiClient.GetPatientProfile(User.Identity.Name)).Data;
            ViewBag.Doctor = (await _doctorApiClient.GetById(doctorid)).Data;
            var getScheduleDoctor = (await _scheduleApiClient.GetScheduleDoctor(doctorid)).Data.ToList();
            ViewBag.GetScheduleDoctor = getScheduleDoctor;
            var slot = await _scheduleApiClient.GetByScheduleSlotId(scheduleid);
            var historyactive = new HistoryActiveCreateRequest()
            {
                ServiceName = NAMESAPACE + ".BookingDoctorSetPatient",
                MethodName = "GET",
                ExtraProperties = slot.IsSuccessed ? "success" : "error",
                Parameters = "{}",
            };
            await HistoryActive(historyactive);
            if (slot.IsSuccessed)
            {
                ViewBag.Date = slot.Data.Schedule.CheckInDate;
                ViewBag.TimeSpan = slot.Data.FromTime.ToString().Substring(0, 5) + "-" + slot.Data.ToTime.ToString().Substring(0, 5);
            }

            var appointmentCreate = new AppointmentCreateRequest()
            {
                DoctorId = doctorid,
                SchedulesSlotId = scheduleid,
                IsDoctor = true,
            };
            return View(appointmentCreate);
        }
        [HttpPost]
        public async Task<IActionResult> BookingDoctorSetPatient(AppointmentCreateRequest request)
        {
            ViewBag.Ethnics = await _userApiClient.GetAllEthnicGroup();
            ViewBag.Province = await _locationApiClient.GetAllProvince(new Guid());
            ViewBag.District = new List<SelectListItem>();
            ViewBag.SubDistrict = new List<SelectListItem>();
            ViewBag.Patient = (await _doctorApiClient.GetPatientProfile(User.Identity.Name)).Data;
            ViewBag.Doctor = (await _doctorApiClient.GetById(request.DoctorId)).Data;
            var getScheduleDoctor = (await _scheduleApiClient.GetScheduleDoctor(request.DoctorId)).Data.ToList();
            ViewBag.GetScheduleDoctor = getScheduleDoctor;
            var slot = await _scheduleApiClient.GetByScheduleSlotId(request.SchedulesSlotId);
            if (slot.IsSuccessed)
            {
                ViewBag.Date = slot.Data.Schedule.CheckInDate;
                ViewBag.TimeSpan = slot.Data.FromTime.ToString().Substring(0, 5) + "-" + slot.Data.ToTime.ToString().Substring(0, 5);
            }
            if (!ModelState.IsValid) return View(request);
            var result = await _appointmentApiClient.Create(request);
            var historyactive = new HistoryActiveCreateRequest()
            {
                ServiceName = NAMESAPACE + ".BookingDoctorSetPatient",
                MethodName = "POST",
                ExtraProperties = result.IsSuccessed ? "success" : "error",
                Parameters = JsonConvert.SerializeObject(request),
            };
            await HistoryActive(historyactive);
            if (!result.IsSuccessed)
            {
                TempData["AlertMessage"] = result.Message;
                TempData["AlertType"] = "error";
                TempData["AlertId"] = "errorToast";
                return View(request);
            }

            TempData["AlertMessage"] = "Đăng nhập thành công.";
            TempData["AlertType"] = "success";
            TempData["AlertId"] = "successToast";
            return RedirectToAction("BookingSuccess", new { Id = result.Data });
        }
        [HttpPost]
        public async Task<IActionResult> AddInfoParent(AddPatientInfoParentRequest request)
        {
            if (!ModelState.IsValid) RedirectToAction("BookingDoctorSetPatient", new { doctorid = request.doctorid, scheduleid = request.scheduleid });
            var add = new AddPatientInfoRequest()
            {
                Address = request.Address,
                DistrictId = request.DistrictId,
                Dob = request.Dob,
                EthnicId = request.EthnicId,
                Gender = request.Gender,
                Identitycard = request.Identitycard,
                LocationId = request.LocationId,
                Name = request.Name,
                ProvinceId = request.ProvinceId,
                RelativeEmail = request.RelativeEmail,
                RelativeName = request.RelativeName,
                RelativePhone = request.RelativePhone,
                UserName = request.UserName,
            };
            await _doctorApiClient.AddInfo(add);
            return RedirectToAction("BookingDoctorSetPatient", new { doctorid = request.doctorid, scheduleid = request.scheduleid });

        }
        [HttpPost]
        public async Task<IActionResult> AddInfoParentBookingClinic(AddPatientInfoParentRequest request)
        {
            if (!ModelState.IsValid) return Json(null);
            var add = new AddPatientInfoRequest()
            {
                Address = request.Address,
                DistrictId = request.DistrictId,
                Dob = request.Dob,
                EthnicId = request.EthnicId,
                Gender = request.Gender,
                Identitycard = request.Identitycard,
                LocationId = request.LocationId,
                Name = request.Name,
                ProvinceId = request.ProvinceId,
                RelativeEmail = request.RelativeEmail,
                RelativeName = request.RelativeName,
                RelativePhone = request.RelativePhone,
                UserName = request.UserName,
            };
            await _doctorApiClient.AddInfo(add);
            var patient = (await _doctorApiClient.GetPatientProfile(User.Identity.Name)).Data;
            return Json(patient);

        }
        [HttpGet]
        public async Task<IActionResult> BookingClinicSetDoctor(Guid clinicid)
        {
            ViewBag.Ethnics = await _userApiClient.GetAllEthnicGroup();
            ViewBag.Province = await _locationApiClient.GetAllProvince(new Guid());
            ViewBag.District = new List<SelectListItem>();
            ViewBag.SubDistrict = new List<SelectListItem>();
            ViewBag.Patient = (await _doctorApiClient.GetPatientProfile(User.Identity.Name)).Data;
            ClinicVm clinc = (await _clinicalApiClient.GetById(clinicid)).Data;
            ViewBag.Clinic = clinc;
            var historyactive = new HistoryActiveCreateRequest()
            {
                ServiceName = NAMESAPACE + ".BookingClinicSetDoctor",
                MethodName = "GET",
                ExtraProperties = "",
                Parameters = "{}",
            };
            await HistoryActive(historyactive);
            if(clinc == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BookingClinicSetDoctor(AppointmentCreateRequest request)
        {
            ViewBag.Ethnics = await _userApiClient.GetAllEthnicGroup();
            ViewBag.Province = await _locationApiClient.GetAllProvince(new Guid());
            ViewBag.District = new List<SelectListItem>();
            ViewBag.SubDistrict = new List<SelectListItem>();
            request.IsDoctor = false;
            ViewBag.Patient = (await _doctorApiClient.GetPatientProfile(User.Identity.Name)).Data;
            ViewBag.Clinic = (await _clinicalApiClient.GetById(request.ClinicId.Value)).Data;

            if (!ModelState.IsValid) return View(request);
            var result = await _appointmentApiClient.Create(request);
            var historyactive = new HistoryActiveCreateRequest()
            {
                ServiceName = NAMESAPACE + ".BookingClinicSetDoctor",
                MethodName = "POST",
                ExtraProperties = result.IsSuccessed ? "success" : "error",
                Parameters = JsonConvert.SerializeObject(request),
            };
            await HistoryActive(historyactive);
            if (result.IsSuccessed)
            {
                return RedirectToAction("BookingSuccess", new { Id = result.Data });
            }
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> BookingSuccess(Guid Id)
        {
            var appointment = await _appointmentApiClient.GetById(Id);
            return View(appointment.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetDoctorScheduleDate(Guid DoctorId, int intMonth)
        {
            if (!string.IsNullOrWhiteSpace(DoctorId.ToString()))
            {
                var schedules = (await _scheduleApiClient.GetScheduleDoctor(DoctorId)).Data.ToList();
                var schedule = new DoctorScheduleClientsVm();
                var dates = new List<ScheduleDoctorDateVm>();
                var dateNow = DateTime.Now.AddMonths(intMonth);
                var day1 = DateTime.Parse("01/" + dateNow.ToString("MM/yyyy"));
                var dateFrom = day1.AddDays(-(day1.DayOfWeek.ToString() == "Moday" ? 0 :
                    day1.DayOfWeek.ToString() == "Tuesday" ? 1 :
                    day1.DayOfWeek.ToString() == "Wednesday" ? 2 :
                    day1.DayOfWeek.ToString() == "Thursday" ? 3 :
                    day1.DayOfWeek.ToString() == "Friday" ? 4 :
                    day1.DayOfWeek.ToString() == "Saturday" ? 5 : 6));
                var dateTo = dateFrom;
                for (var i = 0; i < 42; i++)
                {
                    schedule = schedules.FirstOrDefault(x => x.DateTime == dateTo);
                    if (schedule != null)
                    {
                        schedules = schedules.Where(x => x.Id != schedule.Id).ToList();
                    }
                    var date = new ScheduleDoctorDateVm()
                    {
                        DateTime = dateTo.ToShortDateString(),
                        day = dateTo.Day,
                        DateNow = dateTo.ToShortDateString() == DateTime.Now.ToShortDateString() ? true : false,
                        IsActive = schedule == null ? false : true
                    };
                    dateTo = dateTo.AddDays(1);
                    dates.Add(date);
                }

                return Json(dates);
            }
            return null;
        }
        [HttpGet]
        public async Task<IActionResult> GetDoctorScheduleTimeSpan(Guid DoctorId, string date)
        {
            if (!string.IsNullOrWhiteSpace(DoctorId.ToString()) && !string.IsNullOrWhiteSpace(date))
            {
                var datetime = DateTime.Parse(date);
                var schedule = (await _scheduleApiClient.GetScheduleDoctor(DoctorId)).Data.FirstOrDefault(x => x.DateTime == datetime);
                var sections = new List<SectionScheduleDoctorDateVm>();
                var text = "";
                var img = "";
                for (int i = 1; i <= schedule.CountTimeSpan; i++)
                {
                    var totime = schedule.ScheduleSlots.FirstOrDefault(x => x.Type == i).ToTime.Hours;
                    if (totime < 12)
                    {
                        text = "Buổi sáng";
                        img = "sun.svg";
                    }
                    else if (totime < 18)
                    {
                        text = "Buổi chiều";
                        img = "sun-fog.svg";
                    }
                    else if (totime <= 23)
                    {
                        text = "Buổi tối";
                        img = "sun-fog.svg";
                    }
                    else
                    {
                        text = "Cả ngày";
                        img = "sun.svg";
                    }
                    var section = new SectionScheduleDoctorDateVm()
                    {
                        img = img,
                        text = text,
                        type = i
                    };
                    section.slot = new List<SlotScheduleDoctorDateVm>();
                    foreach (var item in schedule.ScheduleSlots)
                    {
                        if (item.Type == i)
                        {
                            var slot = new SlotScheduleDoctorDateVm()
                            {
                                slotId = item.Id,
                                timeSpan = item.FromTime.ToString().Substring(0, 5) + "-" + item.ToTime.ToString().Substring(0, 5),
                                type = item.Type,
                            };
                            section.slot.Add(slot);
                        }
                    }
                    sections.Add(section);
                }
                return Json(sections);
            }
            return null;
        }
        public IActionResult BookingClinicSetDate()
        {
            return View();
        }
        public IActionResult BookingClinicSetTime()
        {
            return View();
        }
        public async Task<IActionResult> BookingClinicSetPatient(Guid clinicId)
        {
            ViewBag.Patient = (await _doctorApiClient.GetPatientProfile(User.Identity.Name)).Data;
            var clinic = await _clinicalApiClient.GetById(clinicId);
            if (!clinic.IsSuccessed)
            {
                return RedirectToAction("Clinic", "Home");
            }
            return View(clinic.Data);
        }
    }
}

