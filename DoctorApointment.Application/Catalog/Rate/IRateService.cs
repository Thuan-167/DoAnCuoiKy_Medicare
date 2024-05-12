using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorApointment.Data.Entities;
using DoctorApointment.ViewModels.Catalog.Rate;
using DoctorApointment.ViewModels.Common;

namespace DoctorApointment.Application.Catalog.Rate
{
    public interface IRateService
    {
        Task<ApiResult<bool>> Create(RateCreateRequest request);

        Task<ApiResult<bool>> Update(RateUpdateRequest request);

        Task<ApiResult<int>> Delete(Guid Id);

        Task<ApiResult<PagedResult<RatesVm>>> GetAllPaging(GetRatePagingRequest request);

        Task<ApiResult<List<RatesVm>>> GetAll();

        Task<ApiResult<RatesVm>> GetById(Guid Id);
    }
}
