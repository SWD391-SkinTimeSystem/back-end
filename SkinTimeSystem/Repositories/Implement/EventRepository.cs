using BusinessObject.Entities;
using BusinessObject.EventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Interface;
using SharedLibrary.FIleSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    internal class EventRepository : GenericRepository<Event>, IEventRepository
    {
        private readonly FirebaseStorageService _fileService;
        public EventRepository(ApplicationDbContext context,FirebaseStorageService fileService) : base(context) {
            _fileService = fileService;
        }

        public async Task CreateNewEvent(Event @event, IFormFile thumbnail)
        {
            @event.Id = Guid.NewGuid();
            @event.Thumbnail = await _fileService.Upload(thumbnail);
           await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            return await GetAllAsync(x => x.TicketNavigation);
        }

        public async Task<Event?> GetEventById(Guid id)
        {
            return await GetByIdAsync(id, x => x.Include(x => x.TicketNavigation));
        }

        public async Task<PaginationResult<Event>> GetEventPaginated(int page, int pageSize, Expression<Func<Event, bool>> filter)
        {
            return await AsPaginated(page, pageSize, filter, includes: x => x.Include(x => x.TicketNavigation),
                order: x => x.OrderBy(x => x.EventDate).ThenBy(x => x.TimeStart));
        }

        public async Task<PaginationResult<Event>> GetEventWithStatusPaginated(int page, int pageSize, EventStatus status)
        {
            return await AsPaginated(page, pageSize, x => x.Status == status, includes: x => x.Include(x => x.TicketNavigation),
                order: x => x.OrderBy(x => x.EventDate).ThenBy(x => x.TimeStart));
        }
    }
}
