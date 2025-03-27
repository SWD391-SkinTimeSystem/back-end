using API.Model;
using AutoMapper;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
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
        public async Task<IActionResult> GetAllService(string? searchKey, int page = 1, int pageSize = 12)
        {
            var services = await _skinTimeService.GetAllService(searchKey, page, pageSize);
            var result = ServiceResult<PaginationResult<ServiceDTO>>.Success(services);
            return HandleServiceCall(result);
        }

        [HttpGet("availibe-edit")]
        public async Task<IActionResult> GetAllServiceAvailibeEdit(string? searchKey, int page = 1, int pageSize = 12)
        {
            var services = await _skinTimeService.GetAllServiceAvailibeEdit(searchKey, page, pageSize);
            var result = ServiceResult<PaginationResult<ServiceDTO>>.Success(services);
            return HandleServiceCall(result);
        }
        [HttpGet("treatment-plan/{id}")]
        public async Task<IActionResult> GetTreatmentPlan(Guid id)
        {
            return await HandleApiCallAsync(async () =>
            {
                var treatmentPlan = await _skinTimeService.GetTreatmentplant(id);
                return treatmentPlan;
            });
        }

        [HttpGet("treatment-plan")]
        public async Task<IActionResult> GetAllTreatmentPlan()
        {
            return await HandleApiCallAsync(async () =>
            {
                var treatmentPlan = await _skinTimeService.GetAllTreatmentplant();
                return treatmentPlan;
            });
        }
        [HttpPost("basic")]
        public async Task<IActionResult> CreateServiceBasic(ServiceCreateBasicDTO serviceDTO)
        {
            return await HandleServiceCall(async () =>
            {
                return await _skinTimeService.CreateServiceBasic(serviceDTO);

            });
        }
        [HttpPost("advand")]
        public async Task<IActionResult> CreateServiceAdvand(ServiceCreateAdvandDTO serviceDTO)
        {
            return await HandleServiceCall(async () =>
            {
                return await _skinTimeService.CreateServiceAdvand(serviceDTO);  
            });
        }
    }

}
