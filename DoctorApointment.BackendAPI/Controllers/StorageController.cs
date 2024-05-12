using DoctorApointment.Application.Catalog.Post;
using DoctorApointment.ViewModels.Catalog.Post;
using DoctorApointment.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace DoctorApointment.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IPostService _postService;
        public StorageController(IPostService postService)
        {
            _postService = postService;
        }
        /// <summary>
        /// Thêm ảnh
        /// </summary>
        /// 
        [HttpPost("post")]
        public async Task<ActionResult<ApiResult<ImagesVm>>> Create([FromForm] ImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            var result = await _postService.AddImage(request);
            if (!result.IsSuccessed)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
