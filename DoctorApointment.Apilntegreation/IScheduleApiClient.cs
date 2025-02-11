﻿using DoctorApointment.ViewModels.Catalog.Schedule;
using DoctorApointment.ViewModels.Catalog.SlotSchedule;
using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Apilntegreation
{
    public interface IScheduleApiClient
    {
        Task<ApiResult<PagedResult<ScheduleVm>>> GetSchedulePagings(GetSchedulePagingRequest request);
        Task<ApiResult<bool>> Update(ScheduleUpdateRequest request);
        Task<ApiResult<bool>> Create(ScheduleCreateRequest request);

        Task<int> Delete(Guid Id);

        Task<ApiResult<List<ScheduleVm>>> GetAllSchedule();
        Task<ApiResult<List<DoctorScheduleClientsVm>>> GetScheduleDoctor(Guid DoctorId);

        Task<ApiResult<ScheduleVm>> GetById(Guid Id);
        Task<ApiResult<SlotScheduleVm>> GetByScheduleSlotId(Guid Id);
    }
}