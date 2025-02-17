using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkinTime.BLL.Services.SkinTimeService;
using SkinTime.DAL.Entities;
using SkinTime.Models;

namespace SkinTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class serviceController : BaseController
    {
        private readonly ISkinTimeService _skinTimeService;
        private readonly IMapper _mapper;
        public serviceController(ISkinTimeService skinTimeService, IMapper mapper)
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
        [HttpGet("service-details/{serviceId}")]
        public async Task<IActionResult> GetTreatmentPlant([FromRoute] Guid serviceId)
        {

            var result = await _skinTimeService.GetTreatmentPlan(serviceId);

            return Ok(result);
        }
    }
}
