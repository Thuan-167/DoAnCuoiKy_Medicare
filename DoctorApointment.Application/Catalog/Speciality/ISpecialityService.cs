using DoctorApointment.ViewModels.Catalog.Speciality;
using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Application.Catalog.Speciality
{
    public interface ISpecialityService
    {
        Task<ApiResult<bool>> Create(SpecialityCreateRequest request);

        Task<ApiResult<bool>> Update(SpecialityUpdateRequest request);

        Task<ApiResult<int>> Delete(Guid Id);

        Task<ApiResult<PagedResult<SpecialityVm>>> GetAllPaging(GetSpecialityPagingRequest request);

        Task<ApiResult<List<SpecialityVm>>> GetAllSpeciality();

        Task<ApiResult<SpecialityVm>> GetById(Guid Id);
    }
}
