using DoctorApointment.ViewModels.Catalog.Clinic;
using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Apilntegreation
{
    public interface IClinicApiClient
    {
        Task<ApiResult<PagedResult<ClinicVm>>> GetClinicPagings(GetClinicPagingRequest request);
        Task<ApiResult<bool>> Update(ClinicUpdateRequest request);
        Task<ApiResult<bool>> Create(ClinicCreateRequest request);

        Task<int> Delete(Guid Id);
        Task<int> DeleteImg(Guid Id);
        Task<int> DeleteAllImg(Guid Id);
        Task<ApiResult<List<ClinicVm>>> GetMenu();

        Task<ApiResult<ClinicVm>> GetById(Guid Id);
    }
}
