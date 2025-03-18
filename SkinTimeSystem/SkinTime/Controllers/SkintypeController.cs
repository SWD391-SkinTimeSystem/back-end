using API.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;

namespace API.Controllers
{
    [Route("api/skintype")]
    [ApiController]
    public class SkintypeController : BaseController
    {
        private readonly ISkintypeService _skintypeService;
        public SkintypeController(ISkintypeService skintypeService,IMapper mapper, IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities) : base(mapper, emailUtilities, tokenUtilities)
        {
            _skintypeService = skintypeService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllSkintype()
        {
            return await HandleServiceCall(async () =>
            {
                return await _skintypeService.GetAllSkintype();
            });
        }

    }
}
