using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Data;
using SkinTime.BLL.Services.TicketService;
using SkinTime.Extensions;
using SkinTime.Models;
using StackExchange.Redis;

namespace SkinTime.Controllers
{
    [Route("api/ticket")]
    [ApiController]
    public class TicketController : BaseController
    {
        private ITicketService _service;
        private readonly IDatabase _redisCache;

        public TicketController(IMapper mapper, IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities, ITicketService service, IDatabase cache) : base(mapper, emailUtilities, tokenUtilities)
        {
            _service = service;
            _redisCache = cache;
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUserTickets(string? status = null)
        {
            string userId = _tokenUtils.GetDataDictionaryFromJwt(Request.Headers.Authorization.Single()!.Split()[1])["id"];

            return await HandleServiceCall<ICollection<TicketViewModel>>(async () =>
            {
                return await _service.GetAllCustomerTicket(userId, status);
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterServiceTicket([FromBody] TicketRegistrationModel registration)
        {
            string userId = _tokenUtils.GetDataDictionaryFromJwt(Request.Headers.Authorization.Single()!.Split()[1])["id"];

            return await HandleServiceCall(async () =>
            {
                TicketRegistrationCacheModel cachedItem = _mapper.Map<TicketRegistrationCacheModel>(registration);
                cachedItem.UserId = Guid.Parse(userId);

                await _redisCache.SetAsync<TicketRegistrationCacheModel>(cachedItem.Id.ToString(), cachedItem, TimeSpan.FromMinutes(10));

                string callback = Url.Action("TicketTransactionCallback", "Transaction", new { redis = $"{cachedItem.Id}" }, Request.Scheme)!;

                return await _service.CreateTicketForEvent(userId, registration.EventId.ToString(), registration.PaymentMethod, callback);
            });
        }
    }
}
