using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.EventEnums;
using Services.Commons;

namespace Services.Interfaces
{
    public interface IEventService
    {
        Task<ServiceResult<ICollection<Event>>> GetEventList();

        Task<ServiceResult<ICollection<Event>>> GetEventList(Expression<Func<Event, bool>> expression);

        Task<ServiceResult<Event>> GetEventWithId(Guid id);

        Task<ServiceResult<Event>> CreateNewEvent(Event eventInformation);

        Task<ServiceResult<Event>> UpdateEvent(Guid eventId, Event eventInformation);

        Task<ServiceResult> UpdateEventStatus(Guid eventId, EventStatus status);

        Task<ServiceResult> DeleteEvent(string eventId);

        Task<ServiceResult> CancelEvent(string eventId);

        Task<ServiceResult<Event>> GetEventByStatus(EventStatus eventStatus);
    }
}
