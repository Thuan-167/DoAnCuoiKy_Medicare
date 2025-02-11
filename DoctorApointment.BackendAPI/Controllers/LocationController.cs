﻿using DoctorApointment.Application.Catalog.Location;
using DoctorApointment.ViewModels.Catalog.Location;
using DoctorApointment.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorApointment.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        /// <summary>
        /// Tạo mới phường/xã
        /// </summary>
        /// 
        [HttpPost("create-location")]
        [Authorize]
        public async Task<ActionResult<ApiResult<bool>>> Create([FromBody] LocationCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _locationService.Create(request);
            if (!result.IsSuccessed)
                return BadRequest();

            return Ok(result);
        }
        /// <summary>
        /// Xóa phường/xã
        /// </summary>
        /// 
        [HttpDelete("delete/{Id}")]
        [Authorize]
        public async Task<ActionResult<ApiResult<int>>> Delete([FromRoute] Guid Id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _locationService.Delete(Id);

            return Ok(result);
        }
        /// <summary>
        /// Cập nhật phường/xã
        /// </summary>
        /// 
        [HttpPut("update-location")]
        [Authorize]
        public async Task<ActionResult<ApiResult<bool>>> Update([FromBody] LocationUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _locationService.Update(request);
            if (!result.IsSuccessed)
                return BadRequest();
            return Ok();
        }
        /// <summary>
        /// Lấy danh sách phân trang địa chỉ
        /// </summary>
        /// 
        [HttpGet("get-paging-location")]
        public async Task<ActionResult<ApiResult<PagedResult<LocationVm>>>> GetAllPaging([FromQuery] GetLocationPagingRequest request)
        {
            var result = await _locationService.GetAllPaging(request);
            return Ok(result);
        }
        /// <summary>
        /// Lấy tất cả danh sách 
        /// </summary>
        /// 
        [HttpGet("get-paging-all-location")]
        public async Task<ActionResult<ApiResult<List<LocationVm>>>> GetListAllPaging([FromQuery] string? type)
        {
            var result = await _locationService.GetListAllPaging(type);
            return Ok(result);
        }
        /// <summary>
        /// Lấy phường/xã theo id
        /// </summary>
        /// 
        [HttpGet("get-by-id/{Id}")]
        public async Task<ActionResult<ApiResult<LocationVm>>> GetById(Guid Id)
        {
            var result = await _locationService.GetById(Id);
            if (!result.IsSuccessed)
                return BadRequest("Cannot find ward");
            return Ok(result);
        }
        /// <summary>
        /// Lấy tất cả danh sách phường/xã
        /// </summary>
        /// 
        [HttpGet("{districtId}/get-all-subDistrict")]
        public async Task<ActionResult<ApiResult<List<LocationVm>>>> GetAllSubDistrict(Guid districtId)
        {
            var result = await _locationService.GetAllSubDistrict(districtId);
            return Ok(result);
        }
        /// <summary>
        /// Lấy tất cả danh sách quận/huyện
        /// </summary>
        /// 
        [HttpGet("{provinceId}/get-all-district")]
        public async Task<ActionResult<ApiResult<List<LocationVm>>>> GetAllDictrict(Guid provinceId)
        {
            var result = await _locationService.GetAllDistrict(provinceId);
            return Ok(result);
        }
        /// <summary>
        /// Lấy tất cả danh sách quận/huyện thành phố Đà Nẵng
        /// </summary>
        /// 
        [HttpGet("danang-city/get-all-district")]
        public async Task<ActionResult<ApiResult<List<LocationVm>>>> GetAllDistrictDaNangCity()
        {
            var result = await _locationService.GetAllDistrictDaNangCity();
            return Ok(result);
        }
        /// <summary>
        /// Lấy tất cả danh sách tỉnh/thành phố
        /// </summary>
        /// 
        [HttpGet("get-all-province")]
        public async Task<ActionResult<ApiResult<List<LocationVm>>>> GetAllProvince()
        {
            var result = await _locationService.GetAllProvince();
            return Ok(result);
        }
    }
}
