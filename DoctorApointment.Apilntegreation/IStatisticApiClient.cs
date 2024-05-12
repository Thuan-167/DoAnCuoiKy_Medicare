using DoctorApointment.ViewModels.Common;
using DoctorApointment.ViewModels.System.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Apilntegreation
{
    public interface IStatisticApiClient
    {
        Task<List<StatisticActive>> GetServiceFeeStatiticYear(GetHistoryActivePagingRequest request);
        Task<List<StatisticActive>> GetServiceFeeStatiticDay(GetHistoryActivePagingRequest request);
        Task<List<StatisticActive>> GetServiceFeeStatiticMonth(GetHistoryActivePagingRequest request);

        Task<ApiResult<bool>> AddActiveUser(HistoryActiveCreateRequest request);
        Task<List<HistoryActiveVm>> GetAllHistory();
        Task<List<HistoryActiveDetailtVm>> ListActiveUserDetailt();
    }
}
