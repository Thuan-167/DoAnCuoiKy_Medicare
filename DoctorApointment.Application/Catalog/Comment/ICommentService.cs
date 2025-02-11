﻿using DoctorApointment.ViewModels.Catalog.Comment;
using DoctorApointment.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Application.Catalog.Comment
{
    public interface ICommentService
    {
        Task<ApiResult<bool>> Create(CommentCreateRequest request);

        Task<ApiResult<bool>> Update(CommentUpdateRequest request);

        Task<ApiResult<int>> Delete(Guid Id);

        Task<ApiResult<PagedResult<CommentVm>>> GetAllPaging(GetCommentPagingRequest request);

        Task<ApiResult<List<CommentVm>>> GetAll();

        Task<ApiResult<CommentVm>> GetById(Guid Id);
    }
}
