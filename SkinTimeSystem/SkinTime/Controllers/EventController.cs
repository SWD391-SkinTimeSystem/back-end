using API.Model;
using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.EventEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Commons.DTOs.Event;
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
        private readonly IMapper _mapper;
        private readonly IEmailUtilities _emailUtilities;

        public EventController(IMapper mapper, IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities, IEventService services)
            : base(mapper, emailUtilities, tokenUtilities)
        {
            _services = services;
            _mapper = mapper;
            _emailUtilities = emailUtilities;
        }

        [HttpGet("available")]
        [ProducesResponseType<ApiResponse<Collection<AvailableEventDTO>>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<ICollection<AvailableEventDTO>>>> GetAvailableEvents()
        {
            return await HandleServiceCall<ICollection<AvailableEventDTO>>(async () =>
            {
                return await _services.GetEventList(x => x.Status == EventStatus.Approved
                && x.EventDate.ToDateTime(x.TimeStart) > DateTime.UtcNow);
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Event>>> GetEventInformation(Guid id)
        {
            var target = await _services.GetEventWithId(id);

            return await HandleServiceCall<EventDTO>(async () =>
            {
                return await _services.GetEventWithId(id);
            });
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreatEvent([FromBody] EventCreationDTO eventInformation)
        {
            return await HandleServiceCall<EventDTO>(async () =>
        {
            return await _services.CreateNewEvent(_mapper.Map<Event>(eventInformation));
        });
        }

        [HttpPost("state")]
        public async Task<IActionResult> UpdateEventState([FromBody] EventStatusUpdateDTO info)
        {
            return await HandleServiceCall<EventDTO>(async () =>
            {
                return await _services.UpdateEventStatus(info.Id, Enum.Parse<EventStatus>(info.Status));
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CreatEvent(string id)
        {
            return await HandleServiceCall(async () =>
            {
                return await _services.DeleteEvent(id);
            });
        }
    }
}
