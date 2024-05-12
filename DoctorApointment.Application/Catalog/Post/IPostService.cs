using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorApointment.Data.Entities;
using DoctorApointment.ViewModels.Catalog.Post;
using DoctorApointment.ViewModels.Common;

namespace DoctorApointment.Application.Catalog.Post
{
    public interface IPostService
    {
        Task<ApiResult<bool>> Create(PostCreateRequest request);

        Task<ApiResult<bool>> Update(PostUpdateRequest request);

        Task<ApiResult<int>> Delete(Guid Id, bool checkdoctor);

        Task<ApiResult<PagedResult<PostVm>>> GetAllPaging(GetPostPagingRequest request);
        Task<ApiResult<PagedResult<PostVm>>> GetAllPagingAdmin(GetPostPagingRequest request);

        Task<ApiResult<List<PostVm>>> GetAll();

        Task<ApiResult<PostVm>> GetById(Guid Id);

        Task<ApiResult<string>> AddImage(ImageCreateRequest request);
    }
}
