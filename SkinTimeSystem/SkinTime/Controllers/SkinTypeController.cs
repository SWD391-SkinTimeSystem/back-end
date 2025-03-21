using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkinTypeController : BaseController
    {
        ISkintypeService _service;

        public SkinTypeController(IMapper mapper, IEmailUtilities emailUtils, ITokenUtilities tokenUtils, ISkintypeService service)
            : base(mapper, emailUtils, tokenUtils)
        {
            this._service = service;
        }

    }
}
