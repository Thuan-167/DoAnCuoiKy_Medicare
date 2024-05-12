using DoctorApointment.ViewModels.Catalog.Clinic;
using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Application.Catalog.Clinic
{
    public interface IClinicService
    {
        Task<ApiResult<bool>> Create(ClinicCreateRequest request);

        Task<ApiResult<bool>> Update(ClinicUpdateRequest request);

        Task<ApiResult<int>> Delete(Guid Id);
        Task<ApiResult<int>> DeleteImg(Guid Id);
        Task<ApiResult<int>> DeleteAllImg(Guid Id);

        Task<ApiResult<PagedResult<ClinicVm>>> GetAllPaging(GetClinicPagingRequest request);

        Task<ApiResult<List<ClinicVm>>> GetAll();

        Task<ApiResult<ClinicVm>> GetById(Guid Id);
    }
}
