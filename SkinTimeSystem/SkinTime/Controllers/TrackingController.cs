using BusinessObject.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Commons;
using Services.Commons.DTOs.TrackingDTO;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("api/tracking")]
    [ApiController]
    public class TrackingController : BaseController
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
            return await HandleServiceCall(async () =>
            {
                return await _trackingService.CreateTracking(creationalTrackingDTO);
            });

        }

        [Authorize(Roles = nameof(UserRole.Staff))]
        [HttpPut("checkout")]
        public async Task<IActionResult> CheckoutTracking([FromBody] Guid trackingId)
        {
            return await HandleServiceCall(async () =>
            {
                return ServiceResult.Success(await _trackingService.CheckoutTracking(trackingId));
            });
        }

        [Authorize(Roles = nameof(UserRole.Therapist))]
        [HttpPut("note")]
        public async Task<IActionResult> NoteTracking([FromBody] TrackingNoteDTO trackingNoteDTO)
        {
            return await HandleServiceCall(async () =>
            {

                return await _trackingService.NoteTracking(trackingNoteDTO);
            });
        }
    }
}
