using API.Model;
using AutoMapper;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Commons;
using Services.Commons.DTOs.Service;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;

namespace SkinTime.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : BaseController
    {
        private readonly ISkinTimeService _skinTimeService;
        public ServiceController(ISkinTimeService skinTimeService, IMapper mapper, IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities) : base(mapper, emailUtilities, tokenUtilities)
        {
            _skinTimeService = skinTimeService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(Guid id)
        {
            return await HandleServiceCall(async () =>
            {
                var service = await _skinTimeService.GetService(id);                
                return service;
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllService()
        {
            return await HandleServiceCall<ICollection<ServiceDTO>>(async () =>
            {
                var services = await _skinTimeService.GetAllService();
                return ServiceResult<ICollection<Service>>.Success(services);
            });
        }

        [HttpGet("treatment-plan/{id}")]
        public async Task<IActionResult> GetTreatmentPlan(Guid id)
        {
            return await HandleApiCallAsync(async () =>
            {
                var treatmentPlan = await _skinTimeService.GetTreatmentplant(id);
                var treatmentPlanDTO = _mapper.Map<TreatmentPlanDTO>(treatmentPlan);
                return treatmentPlanDTO;
            });
        }

        [HttpGet("treatment-plan")]// 35. Lấy danh sách thông tin của treatment plan 
        public async Task<IActionResult> GetAllTreatmentPlan()
        {
            return await HandleApiCallAsync(async () =>
            {
                var treatmentPlan = await _skinTimeService.GetAllTreatmentplant();
                return Ok(treatmentPlan);
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateService(ServiceCreateDTO serviceDTO)
        {
            return await HandleServiceCall<ApiResponse>(async () =>
            {
                var service = _mapper.Map<Service>(serviceDTO);
                var result = await _skinTimeService.CreateService(service, serviceDTO.ServiceImages, serviceDTO.SkintypeIds);

                if (result.IsSuccess)
                {
                    return ServiceResult<ApiResponse>.Success(new ApiResponse(true, "Service created successfully", null));
                }
                return ServiceResult<ApiResponse>.Failed(new ServiceError(result.Error.Code, result.Error.Description));
            });
        }
    }

}
