using DoctorApointment.Data.Entities;
using DoctorApointment.ViewModels.Common;
using DoctorApointment.ViewModels.System.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Application.System.StatisticService
{
    public interface IStatisticService
    {
        Task<ApiResult<List<HistoryActiveVm>>> ListActiveUser(GetHistoryActivePagingRequest request);
        Task<ApiResult<List<HistoryActiveDetailtVm>>> ListActiveUserDetailt();
        Task<ApiResult<bool>> AddActiveUser(HistoryActiveCreateRequest request);
    }
}
