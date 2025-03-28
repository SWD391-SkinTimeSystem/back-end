using API.Model;
using BusinessObject.Entities;
using BusinessObject.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories;
using Services.Commons;
using Services.Commons.DTOs.TrackingDTO;
using Services.Interfaces;
using System.Drawing.Printing;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutTracking([FromBody] Guid scheduleId)
        {
            return await HandleServiceCall(async () =>
            {
                return await _trackingService.CheckoutTracking(scheduleId);
            });

            
        }


        [Authorize(Roles = nameof(UserRole.Staff))]
        [HttpGet("check/{scheduleID}")]
        public async Task<IActionResult> CheckScheduleWithTrackId(Guid scheduleID)
        {

            return await HandleServiceCall(async () =>
            {
                return ServiceResult.Success(await _trackingService.CheckScheduleWithTrackId(scheduleID));
            });
        }

        [Authorize(Roles = nameof(UserRole.Therapist))]
        [HttpPost("note")]
        public async Task<IActionResult> NoteTracking([FromBody] TrackingNoteDTO trackingNoteDTO)
        {
            return await HandleServiceCall(async () =>
            {

                return await _trackingService.NoteTracking(trackingNoteDTO);
            });
        }

    }
}
