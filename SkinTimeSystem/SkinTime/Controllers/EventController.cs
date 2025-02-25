using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Services.EventService;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum.EventEnums;
using SkinTime.Models;
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

        [HttpGet("available")]
        [ProducesResponseType<ApiResponse<Collection<AvailableEventViewModel>>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<ICollection<AvailableEventViewModel>>>> GetAvailableEvents()
        {
            return await HandleServiceCall<ICollection<AvailableEventViewModel>>(async () =>
            {
                return await _services.GetEventList(x => x.Status == EventStatus.Approved
                && x.EventDate.ToDateTime(x.TimeStart) > DateTime.UtcNow);
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Event>>> GetEventInformation(Guid id)
        {

            return await HandleServiceCall<EventViewModel>(async () =>
            {
                return await _services.GetEventWithId(id);
            });
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreatEvent([FromBody] EventCreationModel eventInformation)
        {
            return await HandleServiceCall<EventViewModel>(async () =>
            {
                return await _services.CreateNewEvent(_mapper.Map<Event>(eventInformation));
            });
        }

        [HttpPost("state")]
        public async Task<IActionResult> UpdateEventState([FromBody] EventStatusUpdateModel info)
        {
            return await HandleServiceCall<EventViewModel>(async () =>
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
