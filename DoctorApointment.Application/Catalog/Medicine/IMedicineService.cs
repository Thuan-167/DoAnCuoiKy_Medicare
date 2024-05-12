using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorApointment.Data.Entities;
using DoctorApointment.ViewModels.Catalog.Medicine;
using DoctorApointment.ViewModels.Common;

namespace DoctorApointment.Application.Catalog.Medicine
{
    public interface IMedicineService
    {
        Task<ApiResult<bool>> Create(MedicineCreateRequest request);

        Task<ApiResult<bool>> Update(MedicineUpdateRequest request);

        Task<ApiResult<int>> Delete(Guid Id);

        Task<ApiResult<PagedResult<MedicineVm>>> GetAllPaging(GetMedicinePagingRequest request);

        Task<ApiResult<List<MedicineVm>>> GetAll(string UserName);

        Task<ApiResult<MedicineVm>> GetById(Guid Id);
    }
}
