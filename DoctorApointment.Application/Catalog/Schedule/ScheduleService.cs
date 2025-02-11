﻿using DoctorApointment.Application.Catalog.Appointment;
using DoctorApointment.Application.System.Users;
using DoctorApointment.Data.EF;
using DoctorApointment.Data.Entities;
using DoctorApointment.Data.Enums;
using DoctorApointment.Utilities.Exceptions;
using DoctorApointment.ViewModels.Catalog.Appointment;
using DoctorApointment.ViewModels.Catalog.Schedule;
using DoctorApointment.ViewModels.Catalog.SlotSchedule;
using DoctorApointment.ViewModels.Common;
using DoctorApointment.ViewModels.System.Models;
using DoctorApointment.ViewModels.System.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;

namespace DoctorApointment.Application.Catalog.Schedule
{
    public class ScheduleService : IScheduleService
    {
        private readonly DoctorManageDbContext _context;
        private readonly IEmailService _emailService;

        public ScheduleService(DoctorManageDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task<ApiResult<bool>> Create(ScheduleCreateRequest request)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == request.Username);
            if (user == null) return new ApiErrorResult<bool>("Tài khoản này không được phép!");
            var manyday = request.ToDay - request.FromDay;
            var manytime = request.ToTime - request.FromTime;
            var minutes = (manytime.Minutes + (manytime.Hours * 60)) / request.Qty;
            var fromtime = request.FromTime;
            var day = request.FromDay;
            //liệt kê tất cả lịch khám của bác sĩ này trong khung ngày fromday-today
            var listschedule = await _context.Schedules.Where(x => x.DoctorId == user.Id && x.CheckInDate >= request.FromDay && x.CheckInDate <= request.ToDay).ToListAsync();
            if (listschedule.Any())
            {
                foreach (var remove in listschedule)
                {
                    //kiểm tra nếu lịch khám có trùng ngày trong tuần nhập vào 
                    if (remove.CheckInDate.DayOfWeek.ToString() == request.WeekDay)
                    {
                        //kiểm tra nếu lịch khám trùng ngày khám lẫn giờ khám 
                        if ((remove.FromTime >= request.FromTime && remove.FromTime < request.ToTime) || (remove.ToTime > request.FromTime && remove.ToTime <= request.ToTime))
                        {
                            //kiểm tra nếu lịch khám đã có người đặt khám thì giữ lại không thì xóa tạo lịch khám mới
                            if (remove.BookedQty > 0)
                                listschedule = listschedule.ToList();
                            else
                            {
                                listschedule = listschedule.Where(x => x.Id != remove.Id).ToList();
                                var removeschedule = await _context.Schedules.FindAsync(remove.Id);
                                _context.Schedules.Remove(removeschedule);
                            }
                        }
                        else listschedule = listschedule.Where(x => x.Id != remove.Id).ToList();
                    }
                }
            }
            for (var i = 1; i <= manyday.Days + 1; i++)
            {
                //lấy phần tử đã kiểm tra ở trên trùng ngày nhập vào và đã có người đặt khám
                var check = listschedule.FirstOrDefault(x => x.CheckInDate.ToShortDateString() == day.ToShortDateString() && x.BookedQty > 0);
                //kiểm tra trùng ngày trong tuần có trong vòng lặp có thì thêm vào
                
                if (day.DayOfWeek.ToString() == request.WeekDay && check == null)
                {
                    var schedules = new Schedules()
                    {
                        FromTime = request.FromTime,
                        ToTime = request.ToTime,
                        CheckInDate = day,
                        IsDeleted = false,
                        Qty = request.Qty,
                        DoctorId = user.Id,
                        BookedQty = 0,
                        AvailableQty = request.Qty,
                        CreatedAt = DateTime.Now
                    };
                    schedules.schedulesSlots = new List<SchedulesSlots>();
                    for (var j = 1; j <= request.Qty; j++)
                    {
                        var scheduledetailt = new SchedulesSlots()
                        {
                            FromTime = fromtime,
                            ToTime = fromtime.Add(TimeSpan.FromMinutes(minutes)),
                            IsDeleted = false,
                            IsBooked = false
                        };
                        schedules.schedulesSlots.Add(scheduledetailt);
                        fromtime = fromtime.Add(TimeSpan.FromMinutes(minutes));
                    }
                    _context.Schedules.Add(schedules);
                }
                day = day.AddDays(1);
            }

            var rs = await _context.SaveChangesAsync();
            if (rs != 0) return new ApiSuccessResult<bool>(true);
            return new ApiErrorResult<bool>("Tạo lịch khám không thành công!!!");
        }

        public async Task<ApiResult<int>> Delete(Guid Id)
        {
            var schedules = await _context.Schedules.FindAsync(Id);
            
            int check = 0;
            if (schedules == null) return new ApiSuccessResult<int>(check);
            var appoi = from slot in _context.schedulesSlots
                        join a in _context.Appointments on slot.Id equals a.SchedulesSlotId
                        join d in _context.Doctors on a.DoctorId equals d.UserId
                        join patient in _context.Patients on a.PatientId equals patient.PatientId
                        join userpe in _context.Users on patient.UserId equals userpe.Id
                        where slot.ScheduleId == schedules.Id
                        select new { slot, a, patient, userpe, d } ;
           
            if (appoi != null)
            {
                appoi.ToListAsync().Result.ForEach(async a => {
                    var appointments = await _context.Appointments.FindAsync(a.a.Id);
                  
                    appointments.Status = StatusAppointment.cancel;
                    appointments.CancelReason = "Bận đột xuất.";
                    appointments.CancelDate = DateTime.Now;
                   
                    var user = await _context.Users.FindAsync(appointments.DoctorId);
                    user.Doctors = a.d;
                    if (user != null && a.patient != null) await SendEmailCancelAppoitment(user, a.userpe, a.patient, schedules, a.slot, appointments);
                });
              
            }
            
            if (schedules.IsDeleted == false)
            {
                schedules.IsDeleted = true;
                check = 1;
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int>(check);
        }

        private async Task SendEmailCancelAppoitment(AppUsers user, AppUsers userpatient, Patients patients, Schedules schedules, SchedulesSlots schedulesSlots, Appointments appointment)
        {
            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email, userpatient.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{DoctorName}}",user.Doctors.Prefix +" " + user.Doctors.LastName + " " + user.Doctors.FirstName),
                    new KeyValuePair<string, string>("{{PatientName}}", patients.Name),
                    new KeyValuePair<string, string>("{{Date}}", schedules.CheckInDate.ToShortDateString()),
                    new KeyValuePair<string, string>("{{CancelReason}}", appointment.CancelReason),
                    new KeyValuePair<string, string>("{{No}}", appointment.No),
                    new KeyValuePair<string, string>("{{CancelDate}}", appointment.CancelDate.ToString("HH:mm:ss dd/MM/yyyy")),
                    new KeyValuePair<string, string>("{{TimeSpan}}", schedulesSlots.FromTime.ToString().Substring(0,5)+"-"+schedulesSlots.ToTime.ToString().Substring(0,5)),
                }
            };
            await _emailService.SendEmailCancelAppoitment(options);
        }

        public async Task<ApiResult<List<ScheduleVm>>> GetAll()
        {
            var query = _context.Schedules;

            var rs = await query.Select(x => new ScheduleVm()
            {
                Id = x.Id,
                CheckInDate = x.CheckInDate,
                FromTime = x.FromTime,
                ToTime = x.ToTime,
                DoctorId = x.DoctorId,
                Qty = x.Qty,
                IsDeleted = x.IsDeleted
            }).ToListAsync();
            return new ApiSuccessResult<List<ScheduleVm>>(rs);
        }
        public async Task<ApiResult<List<DoctorScheduleClientsVm>>> GetScheduleDoctor(Guid DoctorId)
        {
            var doctor = await _context.Doctors.FindAsync(DoctorId);
            var datenow = DateTime.Parse(DateTime.Now.ToShortDateString());
            var query = await _context.Schedules.Where(x => x.DoctorId == DoctorId && x.CheckInDate >= datenow && x.CheckInDate <= datenow.AddDays(doctor.BeforeBookingDay) && x.IsDeleted == false).OrderBy(x => x.CheckInDate).ToListAsync();
            var schedules = from sche in _context.Schedules
                            where sche.DoctorId == DoctorId && sche.CheckInDate >= datenow && sche.CheckInDate <= datenow.AddDays(doctor.BeforeBookingDay) && sche.IsDeleted == false
                            orderby sche.CheckInDate
                            select sche;

            var scheduleClients = new List<DoctorScheduleClientsVm>();
            string d = "";
            var i = 1;
            var datedemo = (DateTime.Now);
            if(datedemo.Hour != 23)
            {
                datedemo.AddHours(1);
            }
            foreach (var item in query)
            {
                var s = new DoctorScheduleClientsVm();
                s.ScheduleSlots = new List<ScheduleSlotsVm>();
                i = 1;
              
                
                foreach (var schedule in schedules.ToList())
                {
                    if (schedule.CheckInDate.ToShortDateString() == item.CheckInDate.ToShortDateString())
                    {
                        if (schedule.CheckInDate.ToShortDateString()==datedemo.ToShortDateString())
                        {
                            TimeSpan time = new TimeSpan(datedemo.Hour, datedemo.Hour == 23? 59: datedemo.Minute, datedemo.Second);
                            var slot = _context.schedulesSlots.Where(x => x.ScheduleId == schedule.Id && x.IsBooked == false && x.IsDeleted == false && x.FromTime > time).ToList();
                            var slotadd = slot.Select(x => new ScheduleSlotsVm()
                            {
                                Id = x.Id,
                                FromTime = x.FromTime,
                                ToTime = x.ToTime,
                                Type = i,
                            }).ToList();
                            s.ScheduleSlots.AddRange(slotadd);
                            i++;
                        }
                        else
                        {
                            var slot = _context.schedulesSlots.Where(x => x.ScheduleId == schedule.Id && x.IsBooked == false && x.IsDeleted == false).ToList();
                            var slotadd = slot.Select(x => new ScheduleSlotsVm()
                            {
                                Id = x.Id,
                                FromTime = x.FromTime,
                                ToTime = x.ToTime,
                                Type = i,
                            }).ToList();
                            s.ScheduleSlots.AddRange(slotadd);
                            i++;
                        }
                        
                    }
                }
                s.CountTimeSpan = i - 1;
                if (!d.Contains(item.CheckInDate.ToShortDateString()))
                {

                    s.DoctorId = item.DoctorId;
                    s.AvailableQty = s.ScheduleSlots.Count;
                    s.DateTime = item.CheckInDate;
                    s.Id = item.Id;
                    scheduleClients.Add(s);
                }

                d = d + ", " + item.CheckInDate.ToShortDateString();

            }
            return new ApiSuccessResult<List<DoctorScheduleClientsVm>>(scheduleClients);
        }

        public async Task<ApiResult<PagedResult<ScheduleVm>>> GetAllPaging(GetSchedulePagingRequest request)
        {
            var query = from c in _context.Schedules select c;
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.CheckInDate.ToShortDateString().Contains(request.Keyword));
            }
            foreach (var item in query.Where(x => x.IsDeleted == false))
            {
                var date = DateTime.Parse(item.CheckInDate.ToShortDateString() + " " + item.FromTime);
                if (item.CheckInDate < DateTime.Now.AddHours(-1))
                {
                    var addstatus = await _context.Schedules.FindAsync(item.Id);
                    addstatus.IsDeleted = true;

                }
            }
            await _context.SaveChangesAsync();
            int totalRow = await query.CountAsync();

            var data = await query.OrderByDescending(x => x.CheckInDate).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ScheduleVm()
                {
                    Id = x.Id,
                    CheckInDate = x.CheckInDate,
                    FromTime = x.FromTime,
                    ToTime = x.ToTime,
                    Qty = x.Qty,
                    IsDeleted = x.IsDeleted,
                    DoctorId = x.DoctorId,
                    AvailableQty = x.AvailableQty,
                    BookedQty = x.BookedQty,
                    ScheduleDetailts = x.schedulesSlots.Select(s => new SlotScheduleVm()
                    {
                        Id = s.Id,
                        FromTime = s.FromTime,
                        ToTime = s.ToTime,
                        IsDeleted = s.IsDeleted,
                        IsBooked = s.IsBooked,
                    }).ToList()

                }).ToListAsync();

            var pagedResult = new PagedResult<ScheduleVm>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<ScheduleVm>>(pagedResult);
        }

        public async Task<ApiResult<ScheduleVm>> GetById(Guid Id)
        {
            var schedule = await _context.Schedules.FindAsync(Id);
            var scheduledetailts = _context.schedulesSlots.Where(x => x.ScheduleId == schedule.Id);
            if (schedule == null) throw new DoctorManageException($"Cannot find a Schedule with id: {Id}");
            var rs = new ScheduleVm()
            {
                Id = schedule.Id,
                CheckInDate = schedule.CheckInDate,
                FromTime = schedule.FromTime,
                ToTime = schedule.ToTime,
                DoctorId = schedule.DoctorId,
                Qty = schedule.Qty,
                IsDeleted = schedule.IsDeleted,
                BookedQty = schedule.BookedQty,
                AvailableQty = schedule.AvailableQty,
                ScheduleDetailts = scheduledetailts.Select(x => new SlotScheduleVm()
                {
                    Id = x.Id,
                    FromTime = x.FromTime,
                    ToTime = x.ToTime,
                    IsDeleted = x.IsDeleted,
                    IsBooked = x.IsBooked,

                }).ToList()
            };
            return new ApiSuccessResult<ScheduleVm>(rs);
        }

        public async Task<ApiResult<bool>> Update(ScheduleUpdateRequest request)
        {
            var schedules = await _context.Schedules.FindAsync(request.Id);
            if (schedules == null) return new ApiErrorResult<bool>(new string[] { "danger", "Lịch khám không tồn tại!!!" });
            if (schedules.BookedQty > 0) return new ApiErrorResult<bool>(new string[] { "warning", "Lịch khám này đã có người đặt khám, cập nhật sau khi hủy đặt khám!!!" });
            if (schedules.FromTime != request.FromTime || schedules.ToTime != request.ToTime
                || schedules.Qty != request.Qty)
            {
                var scheduledetailts = _context.schedulesSlots.Where(x => x.ScheduleId == schedules.Id).ToList();
                foreach (var remove in scheduledetailts)
                {
                    var removescheduledetailt = await _context.schedulesSlots.FindAsync(remove.Id);
                    if (removescheduledetailt != null)
                    {
                        var check_appoint = await _context.Appointments.FirstOrDefaultAsync(x => x.SchedulesSlotId == removescheduledetailt.Id);
                        if (check_appoint != null) removescheduledetailt.IsDeleted = true;
                        else _context.schedulesSlots.Remove(removescheduledetailt);
                    }
                }
                var manytime = request.ToTime - request.FromTime;
                var minutes = (manytime.Minutes + (manytime.Hours * 60)) / request.Qty;
                var fromtime = request.FromTime;
                schedules.schedulesSlots = new List<SchedulesSlots>();
                for (var j = 1; j <= request.Qty; j++)
                {
                    var scheduledetailt = new SchedulesSlots()
                    {
                        FromTime = fromtime,
                        ToTime = fromtime.Add(TimeSpan.FromMinutes(minutes)),
                        IsBooked = false,
                        IsDeleted = false
                    };
                    schedules.schedulesSlots.Add(scheduledetailt);
                    fromtime = fromtime.Add(TimeSpan.FromMinutes(minutes));
                }
            }
            schedules.FromTime = request.FromTime;
            schedules.ToTime = request.ToTime;
            schedules.Qty = request.Qty;
            schedules.AvailableQty = request.Qty;
            schedules.IsDeleted = request.Status;

            var rs = await _context.SaveChangesAsync();
            if (rs != 0) return new ApiSuccessResult<bool>(true);
            return new ApiErrorResult<bool>(new string[] { "warning", "Dự liệu nhập vào không hề thay đổi so với ban đầu!!!" });
        }
    }
}
