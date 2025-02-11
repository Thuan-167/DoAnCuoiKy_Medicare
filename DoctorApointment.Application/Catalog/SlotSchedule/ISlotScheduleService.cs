﻿using DoctorApointment.ViewModels.Catalog.SlotSchedule;
using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Application.Catalog.SlotSchedule
{
    public interface ISlotScheduleService
    {
        Task<ApiResult<bool>> Create(SlotScheduleCreateRequest request);

        Task<ApiResult<bool>> Update(SlotScheduleUpdateRequest request);

        Task<ApiResult<int>> Delete(Guid Id);

        Task<ApiResult<PagedResult<SlotScheduleVm>>> GetAllPaging(GetSlotSchedulePagingRequest request);

        Task<ApiResult<List<SlotScheduleVm>>> GetAll();

        Task<ApiResult<SlotScheduleVm>> GetById(Guid Id);
    }
}
