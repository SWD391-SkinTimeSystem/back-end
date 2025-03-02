using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.DAL.Enum.EventEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.TicketService
{
    public interface ITicketService
    {

        /// <summary>
        ///     Create a payment url for the ticket, the ticket will not be registered until the payment is complete.
        /// </summary>
        /// <param name="userId">the user registrating.</param>
        /// <param name="eventId">the registrating event id</param>
        /// <param name="method">the selected payment method</param>
        /// <param name="callbackUrl">the callback url if the payment is success</param>
        /// <returns></returns>
        Task<ServiceResult<string>> CreateTicketForEvent(string userId, string eventId,string method, string callbackUrl);

        Task<ServiceResult<ICollection<EventTicket>>> GetAllCustomerTicket(string customerId, string? status);

        Task<ServiceResult<EventTicket>> GetTicketWithId(string ticketId);

        Task<ServiceResult<ICollection<EventTicket>>> GetAllEventTicket(string eventId, string? status);

        Task<ServiceResult<EventTicket>> UpdateTicket(string ticketId, EventTicket ticket);

        Task<ServiceResult<string>> CancelEventTicket(string ticketId);
    }
}
