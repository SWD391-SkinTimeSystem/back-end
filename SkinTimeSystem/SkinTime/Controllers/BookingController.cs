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
using SkinTime.BLL.Services.BookingService;
using SkinTime.BLL.Services.ScheduleService;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.Extensions;
using SkinTime.Helpers;
using SkinTime.Models;
using System.Net;
using System.Security.Claims;
using System.Text;
using SkinTime.BLL.Commons;

namespace SkinTime.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : BaseController
    {
        private readonly IBookingService _service;
        private readonly IDatabase _database;

        public BookingController(IDatabase database, IMapper mapper, IEmailUtilities emailUtils, ITokenUtilities tokenUtils, IBookingService bookingService)
        : base(mapper, emailUtils, tokenUtils)
        {
            _database = database;
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
        [Authorize(Roles =  nameof(UserRole.Customer))]
        [HttpPost]
        public async Task<ActionResult<BookingServiceModel>> BookingService(BookingServiceModel booking)
        {
            return await HandleServiceCall (async () =>
            {
                string authHeader = Request.Headers.Authorization.First()!;
                string token = authHeader.Replace("Bearer ", "");
                var tokenData = _tokenUtils.GetDataDictionaryFromJwt(token);

                Guid userId = Guid.Parse(tokenData["id"]);

                Guid bookingId = Guid.NewGuid();

                var bookingData = _mapper.Map<BokingServiceWithIdModel>(booking);
                bookingData.BookingId = bookingId;
                bookingData.UserId = userId;
                
                string redisKey = $"{bookingId}";

                await _database.SetAsync(redisKey, bookingData, TimeSpan.FromMinutes(30));
                var retrievedBooking = await _database.GetAsync<BokingServiceWithIdModel>(redisKey);

                var returnUrl = Url.Action("TransactionCallback", "Transaction", new { redis = redisKey }, Request.Scheme);



                string requestPayment = await _service.CreateNewBooking(returnUrl, booking.ServiceId, booking.PaymentMethod);

                return ServiceResult.Success(requestPayment);


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
