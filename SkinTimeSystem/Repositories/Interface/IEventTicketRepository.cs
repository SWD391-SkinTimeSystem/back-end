using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IEventTicketRepository : IGenericRepository<EventTicket>
    {

        Task<bool> CheckinEventTicket(Guid ticketId);
    }
}
