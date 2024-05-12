using DoctorApointment.Application.System.AnnualServiceFee;
using DoctorApointment.ViewModels.Common;
using DoctorApointment.ViewModels.System.AnnualServiceFee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorApointment.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnualServiceFeeController : ControllerBase
    {
        private readonly IAnnualServiceFeeService _annualServiceFeeService;
        public AnnualServiceFeeController(IAnnualServiceFeeService annualServiceFeeService)
        {
            _annualServiceFeeService = annualServiceFeeService;
        }
        /// <summary>
        /// Lấy danh nộp phí dịch vụ hằng năm phân trang
        /// </summary>
        /// 
        [HttpGet("paging")]
        public async Task<ActionResult<ApiResult<PagedResult<AnnualServiceFeeVm>>>> GetAllPaging([FromQuery] GetAnnualServiceFeePagingRequest request)
        {
            var user = await _annualServiceFeeService.GetAllPaging(request);
            return Ok(user);
        }
        /// <summary>
        /// Lấy nộp phí dịch vụ theo id
        /// </summary>
        /// 
        [HttpGet("{Id}")]
        public async Task<ActionResult<ApiResult<AnnualServiceFeeVm>>> GetById(Guid Id)
        {
            var result = await _annualServiceFeeService.GetById(Id);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("canceled-service-fee")]
        [Authorize]
        public async Task<ActionResult<ApiResult<bool>>> CanceledServiceFee([FromBody] AnnualServiceFeeCancelRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _annualServiceFeeService.CanceledServiceFee(request);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("approved-service-fee/{Id}")]
        [Authorize]
        public async Task<ActionResult<ApiResult<bool>>> ApprovedServiceFee(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _annualServiceFeeService.ApprovedServiceFee(Id);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("payment-service-fee")]
        [Authorize]
        public async Task<ActionResult<ApiResult<bool>>> PaymentServiceFee([FromBody] AnnualServiceFeePaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _annualServiceFeeService.PaymentServiceFee(request);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("payment-service-fee-doctor")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResult<bool>>> PaymentServiceFeeDoctor([FromForm] AnnualServiceFeePaymentDoctorRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _annualServiceFeeService.PaymentServiceFeeDoctor(request);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
