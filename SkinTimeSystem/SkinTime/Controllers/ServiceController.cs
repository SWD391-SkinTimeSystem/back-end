using AutoMapper;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Commons;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.DTOs;
using SkinTime.DTOs.Service;

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
            return await HandleServiceCall<ServiceModel>(async () =>
            {
                var service = await _skinTimeService.GetService(id);

                if (service.Item1 == null)
                {
                    return ServiceResult.Failed(ServiceError.NotFound("Can not find service with provided id"));
                }
                else
                {
                    return ServiceResult.Success(service);
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllService()
        {
            return await HandleServiceCall<ICollection<ServiceModel>>(async () =>
            {
                var services = await _skinTimeService.GetAllService();
                return ServiceResult<ICollection<Service>>.Success(services);
            });
        }

        [HttpGet("treatment-plan/{id}")]// 35. Lấy danh sách thông tin của treatment plan 
        public async Task<IActionResult> GetTreatmentPlan(Guid id)
        {
            return await HandleApiCallAsync(async () =>
            {
                var treatmentPlan = await _skinTimeService.GetTreatmentplant(id);
                var treatmentPlanDTO = _mapper.Map<TreatmentPlanModel>(treatmentPlan);
                return treatmentPlanDTO;
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateService(ServiceDTO serviceDTO)
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
