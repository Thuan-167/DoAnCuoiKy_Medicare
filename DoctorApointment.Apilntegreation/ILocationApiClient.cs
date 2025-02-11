﻿using DoctorApointment.ViewModels.Catalog.Location;
using DoctorApointment.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Apilntegreation
{
    public interface ILocationApiClient
    {
        Task<ApiResult<PagedResult<LocationVm>>> GetLocationPagings(GetLocationPagingRequest request);
        Task<ApiResult<List<LocationVm>>> GetLocationAllPagings(string type);
        Task<ApiResult<bool>> Update(LocationUpdateRequest request);
        Task<ApiResult<bool>> Create(LocationCreateRequest request);

        Task<int> Delete(Guid Id);

        Task<List<SelectListItem>> GetAllSubDistrict(Guid? SubDistricyId, Guid DistricyId);
        Task<List<SelectListItem>> GetAllDistrict(Guid? DistricyId);
        Task<List<SelectListItem>> CityGetAllDistrict(Guid? DistricyId, Guid ProvinceId);

        Task<List<SelectListItem>> GetAllProvince(Guid? ProvinceId);

        Task<ApiResult<LocationVm>> GetById(Guid Id);
    }
}

