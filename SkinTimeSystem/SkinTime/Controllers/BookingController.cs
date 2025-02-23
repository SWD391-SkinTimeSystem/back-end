using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkinTime.BLL.Services.BookingService;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.Helpers;
using SkinTime.Models;
using System.Security.Claims;

namespace SkinTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class bookingController : BaseController
    {
        private readonly IBookingService _service;
        private readonly IMapper _mapper;
        private readonly ITokenUtilities _tokenUtilities;
        public bookingController(IBookingService service,
            IMapper mapper,ITokenUtilities tokenUtilities)
        {
            _mapper = mapper;
            _service = service;
            _tokenUtilities = tokenUtilities;
        }
        [Authorize]
        [HttpGet("{status}")]
        public async Task<IActionResult> GetAppointments([FromRoute] BookingStatus status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out var customerId))
                return BadRequest("Invalid user ID format");

            var result = await _service.GetAppointments(customerId, status);

            return Ok(result);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> BookingService(BookingServiceModel booking)
        {
            string token = Request.Headers.Authorization.First()!;           
            string id = _tokenUtilities.GetDataDictionaryFromJwt(token)["id"];


            if (!Guid.TryParse(id, out var userId))
                return BadRequest();

            var bookingService = _service.CreateNewBooking(userId, booking.serviceId, booking.serviceDate);

            //  var bookingServiceDTO = _mapper.Map<BookingServiceModel>(bookingService);

            //return Ok(new ApiResponse<Booking>
            //{
            //    Success = true,
            //    Data = bookingService,
            //});
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateBookingService(UpdateBookingModel booking)
        {
            var bookingService = _service.UpdateBookingService(booking.BookingId, booking.NewsTimeStart);
            var bookingServiceDTO = _mapper.Map<BookingServiceModel>(bookingService);

            return Ok(new ApiResponse<BookingServiceModel>
            {
                Success = true,
                Data = bookingServiceDTO,
            });
        }


    }
}
