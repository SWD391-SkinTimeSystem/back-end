using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using Services.Interfaces;
using BusinessObject.Entities;
using Services.Commons;
using BusinessObject.Enum;
using Services.Commons.DTOs.Booking;


namespace SkinTime.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : BaseController
    {
        private readonly IBookingService _service;


        public BookingController( IMapper mapper, IEmailUtilities emailUtils, ITokenUtilities tokenUtils, IBookingService bookingService)
        : base(mapper, emailUtils, tokenUtils)
        {
            _service = bookingService;
        }
        [Authorize]
        [HttpGet("status/{status}")]
        public async Task<ActionResult> GetAppointments([FromRoute] string status)
        {
            return await HandleServiceCall(async () => {
                Guid userId = Guid.Parse(GetUserIdFromJwt());
                return ServiceResult.Success(await _service.GetAppointments(userId, status));
            });
        }
        [Authorize(Roles = nameof(UserRole.Customer))]
        [HttpPost]
        public async Task<ActionResult> BookingService(BookingServiceDTO booking)
        {  
            Guid userId = Guid.Parse(GetUserIdFromJwt());
                var returnAction = Url.Action("TransactionCallback", "Transaction",null, Request.Scheme);
            return await HandleServiceCall(async () =>
            {            
                return ServiceResult.Success(await _service.CreateBooking(booking, userId,returnAction));
            });

        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBookingDetails(Guid id)
        {
            return await HandleServiceCall(async () =>
            {
                return ServiceResult.Success(await _service.GetBookingInformation(id));
            });
        }


    }
}
