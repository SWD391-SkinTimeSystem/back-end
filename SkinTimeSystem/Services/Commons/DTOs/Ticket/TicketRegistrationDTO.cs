using Castle.Core.Resource;
using Services.Commons.DTOs.Booking;
using Services.Commons.DTOs.Service;

namespace Services.Commons.DTOs.Ticket
{
    public class TicketRegistrationDTO
    {
        public Guid EventId { get; set; }
        public  decimal Price { get; set; }
        public  string PaymentMethod { get; set; }
        public  string TotalAmount { get; set; }
        public  string SuccessCallbackUrl { get; set; }
        public  string FailureCallbackUrl { get; set; }
    }

    public class TicketRegistrationCacheDTO : TicketRegistrationDTO
    {
        public TicketRegistrationCacheDTO()
        {

        }
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; } 


        public TicketRegistrationCacheDTO(TicketRegistrationDTO dto, Guid eventId,Guid userId)
        {
            UserId = userId;
            EventId = eventId;
            Price = dto.Price;
            TotalAmount = dto.TotalAmount;
            SuccessCallbackUrl = dto.SuccessCallbackUrl;
            FailureCallbackUrl = dto.FailureCallbackUrl;
            PaymentMethod = dto.PaymentMethod;
        }
    }
}
