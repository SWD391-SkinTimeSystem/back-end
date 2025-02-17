﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkinTime.BLL.Services.BookingService;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
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

        public bookingController(IBookingService service,
            IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }
        [HttpGet("{status}")]
        public async Task<IActionResult> GetAppointments([FromRoute] BookingStatus status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out var customerId))
                return BadRequest("Invalid user ID format");

            var result = await _service.GetAppointments(customerId, status);

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> BookingService(BookingServiceModel booking)
        {
            var bookingService = _service.CreateNewBooking(booking.serviceId,booking.serviceDate);
            var bookingServiceDTO = _mapper.Map<BookingServiceModel>(bookingService);

            return Ok(new ApiResponse<BookingServiceModel>
            {
                Success = true,
                Data = bookingServiceDTO,
            });
        }
       


    }
}
