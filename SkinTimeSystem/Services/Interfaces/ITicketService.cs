using BusinessObject.Entities;
using Services.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories;
using Services.Commons.DTOs.Ticket;
using Azure;
using BusinessObject.EventEnums;

namespace Services.Interfaces
{
    public interface ITicketService 
    {

        Task<ServiceResult<string>> CreateTicketForEvent(string userId, string eventId, string method, string callbackUrl);

        Task<ServiceResult<ICollection<EventTicket>>> GetAllCustomerTicket(string customerId, string? status);

        Task<ServiceResult<EventTicket>> GetTicketWithId(string ticketId);

      
        Task<PaginationResult<TicketRegisterListDTO>> GetRegisterEventTicket(Guid eventId, int page, int pageSize);

        Task<PaginationResult<TicketRegisterListDTO>> GetAllEventTicket(Guid eventId, int page, int pageSize);

        Task<ServiceResult<EventTicket>> UpdateTicket(string ticketId, EventTicket ticket);

        Task<ServiceResult<string>> CancelEventTicket(string ticketId);

        Task<ServiceResult> CheckinTicket(Guid eventId, Guid ticketId, string otp);



    }
}
