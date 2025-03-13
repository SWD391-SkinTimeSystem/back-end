using BusinessObject.Entities;
using Services.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITicketService
    {

        Task<ServiceResult<string>> CreateTicketForEvent(string userId, string eventId, string method, string callbackUrl);

        Task<ServiceResult<ICollection<EventTicket>>> GetAllCustomerTicket(string customerId, string? status);

        Task<ServiceResult<EventTicket>> GetTicketWithId(string ticketId);

        Task<ServiceResult<ICollection<EventTicket>>> GetAllEventTicket(string eventId, string? status);

        Task<ServiceResult<EventTicket>> UpdateTicket(string ticketId, EventTicket ticket);

        Task<ServiceResult<string>> CancelEventTicket(string ticketId);
    }
}
