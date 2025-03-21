using API.Model;
using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.EventEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories;
using Services.Commons;
using Services.Commons.DTOs.Event;
using Services.Commons.DTOs.Ticket;
using Services.Implement;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace SkinTime.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : BaseController
    {
        private readonly IEventService _services;


        public EventController(IMapper mapper, IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities, IEventService services)
            : base(mapper, emailUtilities, tokenUtilities)
        {
            _services = services;
        }

        /// <summary>
        ///     Get available events
        /// </summary>
        /// <param name="page">page number, default 1</param>
        /// <param name="pageSize">page size, default 20</param>
        /// <returns></returns>
        [HttpGet("available")]
        [ProducesResponseType<ApiResponse<PaginationResult<AvailableEventDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvailableEvents(int page = 1, int pageSize = 20)
        {
            return Ok(new ApiResponse
            {
                Success = true,
                Data = await _services.GetAvailableEventList(page, pageSize),
                Message = "Success",
            });
        }

        /// <summary>
        ///     Get list of event based on event status
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("status")]
        [ProducesResponseType<ApiResponse<PaginationResult<EventDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventByStatus(int page, int pageSize, EventStatus status)
        {
            return Ok(new ApiResponse
            {
                Success = true,
                Data = await _services.GetEventListWithStatus(page, pageSize, status),
                Message = "Success"
            });
        }

        /// <summary>
        ///     Get event information with provided id
        /// </summary>
        /// <param name="id">event id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType<ApiResponse<EventDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventInformation(Guid id)
        {
            ServiceResult result = await _services.GetEventWithId(id);

            return HandleServiceCall(result);
        }

        /// <summary>
        ///     Create a new event.
        /// </summary>
        /// <param name="eventInformation">event information</param>
        /// <returns>Status 200 <see cref="ApiResponse"/> if success, else status 400</returns>
        [HttpPost("create")]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatEvent([FromBody] EventCreationDTO eventInformation)
        {
            ServiceResult result = await _services.CreateNewEvent(eventInformation);

            return HandleServiceCall(result);
        }

        /// <summary>
        ///     Update event status
        /// </summary>
        /// <param name="info">event update information</param>
        /// <returns></returns>
        [HttpPost("state")]
        public async Task<IActionResult> UpdateEventState([FromBody] EventStatusUpdateDTO info)
        {
            ServiceResult result = await _services.UpdateEventStatus(info.Id, info.Status);

            return HandleServiceCall(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveEvent(Guid id)
        {
            ServiceResult result = await _services.DeleteEvent(id);

            return HandleServiceCall(result);
        }


    }
}
