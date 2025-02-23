using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Services.BookingService;
using SkinTime.BLL.Services.ScheduleService;
using SkinTime.DAL.Entities;
using SkinTime.Models;

namespace SkinTime.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : BaseController
    {
        private IBookingService _service;

        public BookingController(IMapper mapper, IEmailUtilities emailUtils, ITokenUtilities tokenUtils, IBookingService bookingService)
        : base(mapper, emailUtils, tokenUtils)
        {
            this._service = bookingService;
        }

        /// <summary>
        ///     Get all user's booking general informations.
        /// </summary>
        /// <returns>List of created booking</returns>
        [Authorize(Roles = "Customer,Therapist")]
        [HttpGet]
        public async Task<ActionResult<ICollection<BookingViewModel>>> GetPersonalBookings()
        {
            // Get the jwt authorization string from the header.
            string jwt = Request.Headers.Authorization.First()!;
            string user_id = _tokenUtils.GetDataDictionaryFromJwt(jwt.Split()[1])["id"];

            // Get information from the token.
            Dictionary<string,string> tokenData = _tokenUtils.GetDataDictionaryFromJwt(jwt);

            return await HandleServiceCall<ICollection<Booking>, ICollection<BookingViewModel>>(async () =>
            {
                return await _service.GetAllUserBooking(tokenData["id"]);
            });
        }

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
