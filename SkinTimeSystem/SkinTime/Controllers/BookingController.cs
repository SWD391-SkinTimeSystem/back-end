using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;
using System.Threading.Tasks;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.Extensions;
using SkinTime.Helpers;
using System.Net;
using System.Security.Claims;
using System.Text;
using Services.Interfaces;
using SkinTime.DTOs.Booking;
using BusinessObject.Entities;
using Services.Commons;
using BusinessObject.Enum;
using System.Net.WebSockets;

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
        /// <summary>
        ///     Get all user's booking general informations.
        /// </summary>
        /// <returns>List of created booking</returns>
        [Authorize]
        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<BokingServiceStatus>>> GetAppointments([FromRoute] string status)
        {
            return await HandleServiceCall<ICollection<Booking>, List<BokingServiceStatus>>(async () =>
            {
                string authHeader = Request.Headers.Authorization.First()!;
                string token = authHeader.Replace("Bearer ", "");
                var tokenData = _tokenUtils.GetDataDictionaryFromJwt(token);

                Guid userId = Guid.Parse(tokenData["id"]);

                var listBooking = await _service.GetAppointments(userId, status);
                return ServiceResult<ICollection<Booking>>.Success(listBooking);
            });
        }
        [Authorize(Roles = nameof(UserRole.Customer))]
        [HttpPost]
        public async Task<ActionResult<BookingServiceDTO>> BookingService(BookingServiceDTO booking)
        {
            return await HandleServiceCall(async () =>
            {
                Guid userId = Guid.Parse(GetUserIdFromJwt());
                var bookignData = _mapper.Map<Booking>(booking);
                var returnAction = Url.Action("TransactionCallback", "Transaction",null, Request.Scheme);
                return await _service.CreateNewBooking(bookignData, returnAction, booking.ReturnURL,booking.FailureURL,booking.ServiceHour,booking.PaymentMethod, userId);
            });

        }





        //   return Ok(new ApiResponse<BookingServiceModel>
        /// <summary>
        ///     Get a detailed booking information using the booking id.
        /// </summary>
        /// <param name="id">The booking id</param>
        /// <returns>Detailed information of a booking record</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDetailModel>> GetBookingDetails(Guid id)
        {
            return await HandleServiceCall<Booking, BookingDetailModel>(async () =>
            {
                return await _service.GetBookingInformation(id);
            });
        }


    }
}
