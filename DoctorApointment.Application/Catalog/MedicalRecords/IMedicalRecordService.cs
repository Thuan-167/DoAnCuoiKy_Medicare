using DoctorApointment.Data.Entities;
using DoctorApointment.ViewModels.Catalog.MedicalRecords;
using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Application.Catalog.MedicalRecords
{
    public interface IMedicalRecordService
    {
        Task<ApiResult<bool>> Create(MedicalRecordCreateRequest request);

        Task<ApiResult<bool>> Update(MedicalRecordUpdateRequest request);

        Task<ApiResult<int>> Delete(Guid Id);

        Task<ApiResult<PagedResult<MedicalRecordVm>>> GetAllPaging(GetMedicalRecordPagingRequest request);

        Task<ApiResult<List<MedicalRecordVm>>> GetAll();

        Task<ApiResult<MedicalRecordVm>> GetById(Guid Id);
    }
}
