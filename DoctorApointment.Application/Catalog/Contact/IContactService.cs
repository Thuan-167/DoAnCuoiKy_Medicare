using DoctorApointment.ViewModels.Catalog.Contact;
using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Application.Catalog.Contact
{
    public interface IContactService
    {
        Task<ApiResult<bool>> CreateContact(ContactCreateRequest request);

        Task<ApiResult<bool>> UpdateContact(ContactUpdateRequest request);

        Task<ApiResult<int>> DeleteContact(Guid Id);

        Task<ApiResult<PagedResult<ContactVm>>> GetAllPagingContact(GetContactPagingRequest request);

        Task<ApiResult<List<ContactVm>>> GetAllContact();

        Task<ApiResult<ContactVm>> GetByIdContact(Guid Id);
    }
}
