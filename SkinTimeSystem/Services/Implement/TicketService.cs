using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enum;
using BusinessObject.EventEnums;
using Castle.Core.Resource;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories;
using Repositories.Interface;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.Booking;
using Services.Commons.DTOs.Therapist;
using Services.Commons.DTOs.Ticket;
using Services.Interfaces;
using Services.PaymentSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly VNPay _vnPay;
        private readonly ZaloPay _zaloPay;
        private readonly IMapper _mapper;
        private readonly ICache _cache;

        public TicketService(IUnitOfWork unitOfWork, VNPay vnPay, ZaloPay zaloPay, IMapper mapper, ICache cache)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
            _vnPay = vnPay;
            _zaloPay = zaloPay;
            _mapper = mapper;
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

                    switch (ticketInformation.TransactionNavigation!.Method)
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

        public async Task<ServiceResult<string>> CreateTicketForEvent(TicketRegistrationDTO registration, Guid userId, string returnAction)
        {
            var targetEvent = await _unitOfWork.Repository<Event>().GetByIdAsync(registration.EventId);
            if (targetEvent == null)
            {
                return ServiceResult<string>.Failed(ServiceError.NotExisted("Can not get event information with provided event id"));
            }

            if (targetEvent.TicketNavigation.Count() == targetEvent.Capacity)
            {
                return ServiceResult<string>.Failed(ServiceError.ValidationFailed("This event is out of available ticket"));
            }
            var @even = _unitOfWork.Repository<Event>().GetById(registration.EventId);
            var tiketEvent = registration as TicketRegistrationCacheDTO ?? new TicketRegistrationCacheDTO(registration, targetEvent.Id,userId);
            var bank = Enum.TryParse(tiketEvent.PaymentMethod, true, out PaymentMethod pm) && Enum.IsDefined(pm) ? pm : (PaymentMethod?)null;
            string redisKey = $"{Guid.NewGuid()}";
            await _cache.SetAsync(redisKey, JsonConvert.SerializeObject(tiketEvent), TimeSpan.FromMinutes(30));
            returnAction = QueryHelpers.AddQueryString(returnAction, "key", redisKey);
            
            string returnUrl;

            switch (bank)
            {
                case PaymentMethod.ZaloPay:
                    returnUrl = await _zaloPay.CreateZaloPayOrder(targetEvent.TicketPrice, returnAction, $"chuyen khoan ve su kien");
                    break;
                case PaymentMethod.VnPay:
                    returnUrl = await _vnPay.CreateVNPayOrder((int)targetEvent.TicketPrice, returnAction, $"chuyen khoan ve su kien");
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

        public async Task<PaginationResult<TicketRegisterListDTO>> GetRegisterEventTicket(Guid eventId, int page, int pageSize)
        {

            PaginationResult<EventTicket> result = await _unitOfWork.Repository<EventTicket>().AsPaginated(page, pageSize, x => x.EventId == eventId && x.Status == EventTicketStatus.Paid);
            

            return new PaginationResult<TicketRegisterListDTO>
                   {
                        Content = _mapper.Map<ICollection<TicketRegisterListDTO>>(result.Content),
                        CurrentPage = page,
                        ItemAmount = result.ItemAmount,
                        PageSize = pageSize,
                    };
        }

        public async Task<PaginationResult<TicketRegisterListDTO>> GetAllEventTicket(Guid eventId, int page, int pageSize)
        {

            PaginationResult<EventTicket> result = await _unitOfWork.Repository<EventTicket>().AsPaginated(page, pageSize, x => x.EventId == eventId);

            return new PaginationResult<TicketRegisterListDTO>
            {
                Content = _mapper.Map<ICollection<TicketRegisterListDTO>>(result.Content),
                CurrentPage = page,
                ItemAmount = result.ItemAmount,
                PageSize = pageSize,
            };
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

        public async Task<ServiceResult> CheckinTicket(Guid eventId, Guid ticketId, string otp)
        {

            var eventInformation = await _unitOfWork.Repository<Event>().GetByIdAsync(eventId);
            var ticketInformation = await _unitOfWork.Repository<EventTicket>().GetByIdAsync(ticketId);

            var now = DateTime.Now; // Thời gian hiện tại
            //var now = new DateTime(2025, 11, 22, 6, 40, 0);

            // Lấy ngày và giờ sự kiện
            var eventDate = eventInformation.EventDate;
            var timeStart = eventInformation.TimeStart;
            var timeEnd = eventInformation.TimeEnd;

            // Xác định khoảng thời gian cho phép check-in
            var eventStartTime = new DateTime(eventDate.Year, eventDate.Month, eventDate.Day,
                                              timeStart.Hour, timeStart.Minute, timeStart.Second);
            var eventEndTime = new DateTime(eventDate.Year, eventDate.Month, eventDate.Day,
                                            timeEnd.Hour, timeEnd.Minute, timeEnd.Second);
            var checkInStartTime = eventStartTime.AddMinutes(-30); // 30 phút trước TimeStart

            // Kiểm tra điều kiện check-in
            if (now.Date == eventDate.ToDateTime(TimeOnly.MinValue).Date &&
                now >= checkInStartTime &&
                now <= eventEndTime)
            {
                if (ticketInformation.TicketCode == otp)
                {
                    var result = await _unitOfWork.EventTicket.CheckinEventTicket(ticketId);
                    return ServiceResult.Success("✅ Check-in hợp lệ!");
                }
                else
                {
                    return ServiceResult.Failed(ServiceError.ValidationFailed("❌ Mã OTP không hợp lệ!"));
                }
            }
            else
            {
                return ServiceResult.Failed(ServiceError.ValidationFailed("❌ Không thể check-in ngoài khung giờ cho phép!"));
            }

        }
    }
}
