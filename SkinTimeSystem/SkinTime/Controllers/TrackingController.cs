using BusinessObject.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Commons.DTOs.TrackingDTO;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("api/tracking")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingService _trackingService;
        public TrackingController(ITrackingService trackingService)
        {
            _trackingService = trackingService;
        }
        [Authorize(Roles = nameof(UserRole.Staff))]
        [HttpPost("checkin")]
        public async Task<IActionResult> TrackingBooking(CreationalTrackingDTO creationalTrackingDTO)
        {
            _trackingService.CreateTracking(creationalTrackingDTO);
            return Ok();

        }

        //[Authorize(Roles = nameof(UserRole.Staff))]
        [HttpPut("checkout")]
        public async Task<IActionResult> CheckoutTracking(Guid trackingId)
        {
            _trackingService.CheckoutTracking(trackingId);
            return Ok();
        }

//[Authorize(Roles = nameof(UserRole.Therapist))]
        [HttpPut("note")]
        public async Task<IActionResult> NoteTracking(Guid trackingId, string note)
        {
            _trackingService.NoteTracking(trackingId, note);
            return Ok();
        }
    }
}
