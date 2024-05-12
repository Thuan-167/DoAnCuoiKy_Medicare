using DoctorApointment.Data.Entities;
using DoctorApointment.ViewModels.Common;
using DoctorApointment.ViewModels.System.Doctors;
using DoctorApointment.ViewModels.System.Patient;
using DoctorApointment.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Application.System.Doctor
{
    public interface IDoctorService
    {

        Task<ApiResult<List<DoctorVm>>> GetTopFavouriteDoctors();
        Task<ApiResult<DoctorVm>> GetById(Guid id);
        Task<ApiResult<PatientVm>> GetByPatientId(Guid id);
        Task<ApiResult<bool>> UpdateInfo(UpdatePatientInfoRequest request);
        Task<ApiResult<Guid>> AddInfo(AddPatientInfoRequest request);
        Task<ApiResult<List<PatientVm>>> GetPatientProfile(string username);
        Task<ApiResult<List<UserVm>>> GetAllUser(string? role);
    }
}
