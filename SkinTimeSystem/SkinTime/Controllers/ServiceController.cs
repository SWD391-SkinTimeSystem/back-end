using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Services.SkinTimeService;
using SkinTime.DAL.Entities;
using SkinTime.Helpers;
using SkinTime.Models;

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
            _mapper = mapper;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(Guid id)
        {
            return await HandleApiCallAsync(async () =>
            {
                var service = await _skinTimeService.GetService(id);
                var serviceDTO = _mapper.Map<ServiceModel>(service);
                return serviceDTO;
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllService()
        {
            return await HandleApiCallAsync(async () =>
            {
                var service = _skinTimeService.GetAllService;
                var serviceDTO = _mapper.Map<ServiceModel>(service);
                return serviceDTO;
            });
        }
        [HttpGet("treatment-plan/{id}")]// 35. Lấy danh sách thông tin của treatment plan 
        public async Task<IActionResult> GetTreatmentPlan(Guid id)
        {
            return await HandleApiCallAsync(async () =>
            {
                var treatmentPlan = await _skinTimeService.GetTrementplant(id);
                var treatmentPlanDTO =  _mapper.Map<TreatmentPlanModel>(treatmentPlan);
                return treatmentPlanDTO;
            });
        }
    }

}
