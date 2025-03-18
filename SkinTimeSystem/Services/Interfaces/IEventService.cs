using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.EventEnums;
using Services.Commons;
using Repositories;
using Services.Commons.DTOs.Event;

namespace Services.Interfaces
{
    public interface IEventService
    {
        Task<ICollection<EventDTO>> GetAllEvents();

        Task<ServiceResult<EventDTO>> GetEventWithId(Guid id);

        Task<PaginationResult<EventDTO>> GetEventListWithStatus(int page, int pageSize, EventStatus status);

        Task<PaginationResult<EventDTO>> GetEventList(int page, int pageSize, Expression<Func<Event, bool>> expression);

        Task<PaginationResult<AvailableEventDTO>> GetAvailableEventList(int page, int pageSize);

        Task<ServiceResult> CreateNewEvent(EventCreationDTO eventInformation);

        Task<ServiceResult> UpdateEvent(Guid eventId, EventUpdateDTO eventInformation);

        Task<ServiceResult> UpdateEventStatus(Guid eventId, EventStatus status);

        Task<ServiceResult> DeleteEvent(Guid id);

        Task<ServiceResult> CancelEvent(Guid id);
    }
}
