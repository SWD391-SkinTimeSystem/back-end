using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkinTime.BLL.Services.EventService;
using SkinTime.BLL.Services.UserService;
using SkinTime.DAL.Entities;
using SkinTime.Helpers;
using SkinTime.Models;
using System.Linq.Expressions;

namespace SkinTime.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _services;
        private readonly IMapper _mapper;
        private readonly IEmailUtilities _emailUtilities;

        public EventController(IEventService services, IMapper mapper, IEmailUtilities emailUtilities)
        {
            _services = services;
            _mapper = mapper;
            _emailUtilities = emailUtilities;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableEvents()
        {
            // Available events are events that's still have tickets and isn't started yet.
            Expression<Func<Event, bool>> filterExpression = x => x.TimeStart > DateTime.UtcNow && x.EventTickets.Count < x.Capacity;
            var filteredEvents = await _services.GetEventList(filterExpression);

            // Create an api response and map the result to data.
            ApiResponse<ICollection<EventInformation>> response = new()
            {
                Success = true,
                Data = _mapper.Map<ICollection<EventInformation>>(filteredEvents)
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventInformation(Guid id)
        {
            var target = await _services.GetEventWithId(id);

            // Create the api response when failure.
            ApiResponse<object> response = new()
            {
                Success = false,
                Data = "Event does not exist for the provided id"
            };

            if (target != null)
            {
                // Update the response if the event found.
                response.Success = true;
                response.Data = _mapper.Map<EventInformation>(target);

                return Ok(response);
            }

            return NotFound(response);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreatEvent([FromBody] EventCreation eventInformation)
        {
            var entityObject = _mapper.Map<Event>(eventInformation);

            var result = await _services.CreateNewEvent(entityObject);

            // Create the api response when failure.
            ApiResponse<object> response = new()
            {
                Success = false,
                Data = "Failed while trying to create the event"
            };

            if (result != null)
            {
                response.Success = true;
                response.Data = "Successfully created new event";

                return Created((string?)null, response);
            }

            return BadRequest(response);
        }
    }
}
