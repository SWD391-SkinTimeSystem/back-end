using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Services.BookingService;
using SkinTime.BLL.Services.ScheduleService;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.Helpers;
using SkinTime.Models;
using System.Security.Claims;

namespace SkinTime.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : BaseController
    {
        private readonly IBookingService _service;

        public BookingController(IMapper mapper, IEmailUtilities emailUtils, ITokenUtilities tokenUtils, IBookingService bookingService)
        : base(mapper, emailUtils, tokenUtils)
        {
            _service = bookingService;
        }
        /// <summary>
        ///     Get all user's booking general informations.
        /// </summary>
        /// <returns>List of created booking</returns>
        //[Authorize]
        //[HttpGet("{status}")]
        //public async Task<IActionResult> GetAppointments([FromRoute] BookingStatus status)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (!Guid.TryParse(userId, out var customerId))
        //        return BadRequest("Invalid user ID format");

        //    var result = await _service.GetAllUserBooking(customerId, status);

        //    return Ok(result);
        //}
        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> BookingService(BookingServiceModel booking)
        //{
        //    Get the jwt authorization string from the header.
        //    string jwt = Request.Headers.Authorization.First()!;
        //    string user_id = _tokenUtils.GetDataDictionaryFromJwt(jwt.Split()[1])["id"];
        //    string token = Request.Headers.Authorization.First()!;
        //    string id = _tokenUtils.GetDataDictionaryFromJwt(token)["id"];


        //    if (!Guid.TryParse(id, out var userId))
        //        return BadRequest();

        //    var bookingService = _service.CreateNewBooking(userId, booking.serviceId, booking.serviceDate);

        //    Get information from the token.
        //   Dictionary<string, string> tokenData = _tokenUtils.GetDataDictionaryFromJwt(jwt);
        //    var bookingServiceDTO = _mapper.Map<BookingServiceModel>(bookingService);

        //    return await HandleServiceCall<ICollection<Booking>, ICollection<BookingViewModel>>(async () =>
        //    {
        //        var bookingService = _service.UpdateBookingService(booking.BookingId, booking.NewsTimeStart);
        //        var bookingServiceDTO = _mapper.Map<BookingServiceModel>(bookingService);
        //        return await _service.GetAllUserBooking(tokenData["id"]);
        //    });
        //}

         //   return Ok(new ApiResponse<BookingServiceModel>
        /// <summary>
        ///     Get a detailed booking information using the booking id.
        /// </summary>
        /// <param name="id">The booking id</param>
        /// <returns>Detailed information of a booking record</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDetailViewModel>> GetBookingDetails(string id)
        {
            return await HandleServiceCall<Booking, BookingDetailViewModel>(async () =>
            {
                return await _service.GetBookingInformation(id);
            });
        }


    }
}
