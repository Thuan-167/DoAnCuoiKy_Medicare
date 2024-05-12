using DoctorApointment.ViewModels.Catalog.MasterData;
using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Apilntegreation
{
    public interface IMasterDataApiClient
    {
        Task<ApiResult<InformationVm>> GetById();
        Task<ApiResult<bool>> Update(InformationUpdateRequest request);
        Task<ApiResult<bool>> CreateMainMenu(MainMenuCreateRequest request);

        Task<ApiResult<bool>> UpdateMainMenu(MainMenuUpdateRequest request);

        Task<int> DeleteMainMenu(Guid Id);

        Task<ApiResult<PagedResult<MainMenuVm>>> GetAllPagingMainMenu(GetMainMenuPagingRequest request);

        Task<ApiResult<List<MainMenuVm>>> GetAllMainMenu();

        Task<ApiResult<MainMenuVm>> GetByIdMainMenu(Guid Id);

        Task<ApiResult<bool>> CreateEthnic(EthnicCreateRequest request);

        Task<ApiResult<bool>> UpdateEthnic(EthnicUpdateRequest request);

        Task<int> DeleteEthnic(Guid Id);

        Task<ApiResult<PagedResult<EthnicsVm>>> GetAllPagingEthnic(GetEthnicPagingRequest request);

        Task<ApiResult<List<EthnicsVm>>> GetAllEthnic();

        Task<ApiResult<EthnicsVm>> GetByIdEthnic(Guid Id);
    }
}
