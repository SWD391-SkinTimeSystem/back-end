using BusinessObject.Entities;
using BusinessObject.EventEnums;
using Repositories.Data;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    public class EventTicketRepository : GenericRepository<EventTicket>, IEventTicketRepository
    {
        public EventTicketRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> CheckinEventTicket(Guid ticketId)
        {
            var ticket = _context.EventTickets.SingleOrDefault(x => x.Id == ticketId);
            ticket.Status = EventTicketStatus.CheckedIn;
            _context.EventTickets.Update(ticket);
            await _context.SaveChangesAsync();
            return true;


        }
    }
}
