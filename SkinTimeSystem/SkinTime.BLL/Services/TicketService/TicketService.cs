using Castle.Core.Resource;
using Cursus.Core.Options.PaymentSetting;
using Microsoft.EntityFrameworkCore;
using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.DAL.Enum.EventEnums;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.TicketService
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly VNPay _vnPay;
        private readonly ZaloPay _zaloPay;

        public TicketService(IUnitOfWork unitOfWork, VNPay vnPay, ZaloPay zaloPay)
        {
            _unitOfWork = unitOfWork;
            _vnPay = vnPay;
            _zaloPay = zaloPay;
        }

        public async Task<ServiceResult<string>> CancelEventTicket(string ticketId)
        {
            if (Guid.TryParse(ticketId, out var parsedId))
            {
                var ticketInformation = await _unitOfWork.Repository<EventTicket>().GetByIdAsync(parsedId, x => 
                x.Include(x => x.EventNavigation)
                .Include(x => x.TransactionNavigation));

                if (ticketInformation == null)
                {
                    return ServiceResult<string>
                        .Failed(ServiceError.NotFound("Can not find the required resource with the provided id"));
                }


                if (ticketInformation.EventNavigation.Status != EventStatus.Approved || ticketInformation.Status != EventTicketStatus.Paid)
                {
                    return ServiceResult<string>
                        .Failed(ServiceError.ValidationFailed("This ticket does not valid for cancelation"));
                }

                ticketInformation.Status = EventTicketStatus.Canceled;
                ticketInformation = _unitOfWork.Repository<EventTicket>().Update(ticketInformation);

                DateTime eventStart = ticketInformation.EventNavigation.EventDate.ToDateTime(ticketInformation.EventNavigation.TimeStart);

                if ((eventStart - DateTime.UtcNow).TotalHours > 48)
                {
                    string refundUrl = string.Empty;
                    
                    switch(ticketInformation.TransactionNavigation!.Method)
                    {
                        case PaymentMethod.VnPay:
                            refundUrl = "";
                            break;
                        case PaymentMethod.ZaloPay:
                            refundUrl = "";
                            break;
                    }

                    return ServiceResult<string>.Success(refundUrl);
                }

                return ServiceResult<string>.Success("");
            }

            return ServiceResult<string>
                .Failed(ServiceError.ValidationFailed("The given id does not match the required format"));
        }

        public async Task<ServiceResult<string>> CreateTicketForEvent(string userId, string eventId, string paymentMethod, string callbackUrl)
        {
            // Validate input
            if (!Guid.TryParse(userId, out Guid parseUserId))
            {
                return ServiceResult<string>.Failed(ServiceError.ValidationFailed("user id does not match the required format")); ;
            }

            if (!Enum.TryParse<PaymentMethod>(paymentMethod, true, out PaymentMethod parsedMethod))
            {
                return ServiceResult<string>.Failed(ServiceError.ValidationFailed("invalid payment method"));
            }

            if (!Guid.TryParse(eventId, out Guid parsedEventId))
            {
                return ServiceResult<string>.Failed(ServiceError.ValidationFailed("event id does not match the required format")); ;
            }

            // Get data from database for processing
            var targetEvent = await _unitOfWork.Repository<Event>().GetByIdAsync(parsedEventId, x => x.Include(x => x.TicketNavigation));

            // Processing data
            if (targetEvent == null)
            {
                return ServiceResult<string>.Failed(ServiceError.NotExisted("Can not get event information with provided event id"));
            }

            if (targetEvent.TicketNavigation.Count() == targetEvent.Capacity)
            {
                return ServiceResult<string>.Failed(ServiceError.ValidationFailed("This event is out of available ticket"));
            }

            // Create payment url
            string returnUrl;

            switch(parsedMethod)
            {
                case PaymentMethod.ZaloPay:
                    returnUrl = await _zaloPay.CreateZaloPayOrder(targetEvent.TicketPrice, callbackUrl, $"chuyen khoan ve su kien");
                    break;
                case PaymentMethod.VnPay:
                    returnUrl = await _vnPay.CreateVNPayOrder((int) targetEvent.TicketPrice, callbackUrl, $"chuyen khoan ve su kien");
                    break;
                default:
                    return ServiceResult<string>.Failed(ServiceError.ValidationFailed("Payment method does not supported for this type of action"));
            };

            return ServiceResult<string>.Success(returnUrl);
        }

        public async Task<ServiceResult<ICollection<EventTicket>>> GetAllCustomerTicket(string customerId, string? status = null)
        {
            // Input parameters validation
            if (!Guid.TryParse(customerId, out var parsedCustomerId))
            {
                return ServiceResult<ICollection<EventTicket>>.Failed(ServiceError.ValidationFailed("User id does not match the required"));
            }

            if (!_unitOfWork.Repository<User>().Exists(parsedCustomerId))
            {
                return ServiceResult<ICollection<EventTicket>>.Failed(ServiceError.NotExisted("Can not find user information with the provided id"));
            }

            IEnumerable<EventTicket> userTickets = await _unitOfWork.Repository<EventTicket>().ListAsync(x => x.Include(x => x.EventNavigation), x => x.UserID == parsedCustomerId);

            if (status != null)
            {
                if (!Enum.TryParse<EventTicketStatus>(status, out var statusValue))
                {
                    return ServiceResult<ICollection<EventTicket>>.Failed(ServiceError.ValidationFailed("the given status is invalid"));
                }

                userTickets = userTickets.Where(x => x.Status == statusValue);
            }

            return ServiceResult<ICollection<EventTicket>>.Success(userTickets.ToList()); 
        }

        public async Task<ServiceResult<ICollection<EventTicket>>> GetAllEventTicket(string eventId, string? status)
        {
            // Input parameters validation
            if (!Guid.TryParse(eventId, out var parsedEventId))
            {
                return ServiceResult<ICollection<EventTicket>>.Failed(ServiceError.ValidationFailed("User id does not match the required"));
            }

            IEnumerable<EventTicket> eventTickets = await _unitOfWork.Repository<EventTicket>().ListAsync(x => x.EventId == parsedEventId);

            if (status != null)
            {
                if (!Enum.TryParse<EventTicketStatus>(status, out var statusValue))
                {
                    return ServiceResult<ICollection<EventTicket>>.Failed(ServiceError.ValidationFailed("the given status is invalid"));
                }

                eventTickets = eventTickets.Where(x => x.Status == statusValue);
            }

            return ServiceResult<ICollection<EventTicket>>.Success(eventTickets.ToList());
        }

        public async Task<ServiceResult<EventTicket>> GetTicketWithId(string ticketId)
        {
            if (Guid.TryParse(ticketId, out var parsedId))
            {
                var ticketInformation = await _unitOfWork.Repository<EventTicket>().GetByIdAsync(parsedId);

                if (ticketInformation == null)
                {
                    return ServiceResult<EventTicket>
                        .Failed(ServiceError.NotFound("Can not find the required resource with the provided id"));
                }

                return ServiceResult<EventTicket>.Success(ticketInformation);
            }

            return ServiceResult<EventTicket>
                .Failed(ServiceError.ValidationFailed("The given id does not match the required format"));
        }

        public Task<ServiceResult<EventTicket>> UpdateTicket(string ticketId, EventTicket ticket)
        {
            throw new NotImplementedException();
        }
    }
}
