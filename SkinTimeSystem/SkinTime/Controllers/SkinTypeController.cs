using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Commons.DTOs.SkinType;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;

namespace API.Controllers
{
    [Route("api/skinType")]

    [ApiController]
    public class SkinTypeController : BaseController
    {
        ISkintypeService _service;

        public SkinTypeController(IMapper mapper, IEmailUtilities emailUtils, ITokenUtilities tokenUtils, ISkintypeService service)
            : base(mapper, emailUtils, tokenUtils)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> CreateSkintype(SkinTypeCreationDTO serviceDTO)
        {
            return await HandleServiceCall(async () =>
            {
                return await _service.CreateSkinType(serviceDTO);

            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSkintype()
        {
            return await HandleServiceCall(async () =>
            {
                return await _service.GetAllSkintype();

            });
        }
    }
}
