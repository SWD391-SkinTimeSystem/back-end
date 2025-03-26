using API.Model;
using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.EventEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Services.Commons;
using Services.Commons.DTOs.Booking;
using Services.Commons.DTOs.Therapist;
using Services.Commons.DTOs.Ticket;
using Services.Implement;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.Extensions;
using StackExchange.Redis;
using System.Drawing.Printing;

namespace SkinTime.Controllers
{
    [Route("api/ticket")]
    [ApiController]
    public class TicketController : BaseController
    {
        private ITicketService _service;

        public TicketController(IMapper mapper, IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities, ITicketService service) : base(mapper, emailUtilities, tokenUtilities)
        {
            _service = service;
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


        [Authorize(Roles = "Staff")]
        [HttpGet("{eventId}/available-list")]
        [ProducesResponseType<ApiResponse<PaginationResult<TicketRegisterListDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventTicketRegisterList( Guid eventId, int page = 1, int pageSize = 20)
        {

            PaginationResult result = await _service.GetRegisterEventTicket(eventId, page, pageSize);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "success",
                Data = result,
            });
        }


        [Authorize(Roles = "Staff")]
        [HttpGet("{eventId}/list")]
        [ProducesResponseType<ApiResponse<PaginationResult<TicketRegisterListDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventTicketList( Guid eventId, int page = 1, int page_size = 20)
        {

            PaginationResult result = await _service.GetAllEventTicket(eventId, page, page_size);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "success",
                Data = result,
            });
        }


        [Authorize(Roles = "Staff")]
        [HttpPost("{ticketId}/checkin")]
        public async Task<IActionResult> CheckInTicket([FromQuery]Guid eventId,Guid ticketId, [FromQuery] string otp)
        {
            return await HandleServiceCall(async () =>
            {
                return await _service.CheckinTicket(eventId, ticketId, otp);
            });
        }
        [Authorize(Roles = nameof(UserRole.Customer))]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterServiceTicket(TicketRegistrationDTO registration)
        {
            Guid userId = Guid.Parse(GetUserIdFromJwt());
            var returnAction = Url.Action("TicketTransactionCallback", "Transaction", null, Request.Scheme);
            return await HandleServiceCall(async () =>
            {
                return await _service.CreateTicketForEvent(registration, userId, returnAction);
            });

        }
    }
}
