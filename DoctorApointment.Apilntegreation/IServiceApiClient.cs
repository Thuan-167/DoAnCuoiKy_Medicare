﻿using DoctorApointment.ViewModels.Catalog.Service;
using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Apilntegreation
{
    public interface IServiceApiClient
    {
        Task<ApiResult<bool>> Create(ServiceCreateRequest request);

        Task<ApiResult<bool>> Update(ServiceUpdateRequest request);

        Task<int> Delete(Guid Id);

        Task<ApiResult<PagedResult<ServiceVm>>> GetAllPaging(GetServicePagingRequest request);

        Task<ApiResult<List<ServiceVm>>> GetAll(string UserName);

        Task<ApiResult<ServiceVm>> GetById(Guid Id);
    }
}

