using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.EventService
{
    public interface IEventService
    {
        Task<ICollection<Event>> GetEventList();

        Task<ICollection<Event>> GetEventList(Expression<Func<Event,bool>> expression);

        Task<Event?> GetEventWithId(Guid id);

        Task<Event?> CreateNewEvent(Event eventInformation);

        Task<Event> UpdateEvent(Guid eventId, Event eventInformation);

        Task<Event> DeleteEvent(Guid eventId);
    }
}
