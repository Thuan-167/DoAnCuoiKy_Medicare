using DoctorApointment.ViewModels.Catalog.Speciality;
using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Apilntegreation
{
    public interface ISpecialityApiClient
    {
        Task<ApiResult<PagedResult<SpecialityVm>>> GetSpecialityPagings(GetSpecialityPagingRequest request);
        Task<ApiResult<bool>> Update(SpecialityUpdateRequest request);
        Task<ApiResult<bool>> Create(SpecialityCreateRequest request);

        Task<int> Delete(Guid Id);

        Task<ApiResult<List<SpecialityVm>>> GetAllSpeciality();

        Task<ApiResult<SpecialityVm>> GetById(Guid Id);
    }
}
