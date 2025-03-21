using BusinessObject.EventEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.Ticket
{
    public class TicketRegisterListDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; } = Guid.Empty;

        public string Ticket_Otp { get; set; } = string.Empty;
        public string Base64_QrCode { get; set; } = string.Empty;

        public EventTicketStatus Status { get; set; } 
    }
}
