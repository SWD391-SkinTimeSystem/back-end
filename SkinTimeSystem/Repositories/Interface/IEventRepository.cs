using BusinessObject.Entities;
using BusinessObject.EventEnums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IEventRepository: IGenericRepository<Event>
    {
        Task<IEnumerable<Event>> GetAllEvents();

        Task<Event?> GetEventById(Guid id);
        Task CreateNewEvent(Event @event,IFormFile thumbnail );

        Task<PaginationResult<Event>> GetEventWithStatusPaginated(int page, int pageSize, EventStatus status);

        Task<PaginationResult<Event>> GetEventPaginated(int page, int pageSize, Expression<Func<Event, bool>> filter);
    }
}
