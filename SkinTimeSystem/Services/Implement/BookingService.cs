using BusinessObject.Entities;
using BusinessObject.Enum;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Repositories.ConcreteRepository.Interface;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Interfaces;
using Services.PaymentSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly VNPay _vnPay;
        private readonly ZaloPay _zaloPay;
        private readonly ICache _cache;
        public BookingService(ICache cache,IUnitOfWork unitOfWork, VNPay vNPay, ZaloPay zaloPay)
        {
            _unitOfWork = unitOfWork;
            _vnPay = vNPay;
            _zaloPay = zaloPay;
            _cache = cache;
        }

        public async Task<ServiceResult<string>> CreateNewBooking(Booking booking, string returnAction, string returnURL, string failureURL, TimeOnly serviceHour, string paymentMethod, Guid userId)
        {
            var service = _unitOfWork.Repository<Service>().GetById(booking.ServiceId);
            var bank = Enum.TryParse(paymentMethod, true, out PaymentMethod pm) && Enum.IsDefined(pm) ? pm : (PaymentMethod?)null;

            booking.Id = Guid.NewGuid();
            booking.CustomerId = userId;

            var bookingData = new
            {
                Booking = booking,
                ServiceHour = serviceHour,
                PaymentMethod = paymentMethod,
                ReturnURL = returnURL,
                FailureURL = failureURL,
            };
            string redisKey = $"booking:{booking.Id}";
            await _cache.SetAsync(redisKey, bookingData, TimeSpan.FromMinutes(30));
            returnAction = QueryHelpers.AddQueryString(returnAction, "redisKey", redisKey);

            string? result = null;

            switch (bank)
            {
                case PaymentMethod.VnPay:
                    result = await _vnPay.CreateVNPayOrder((int)service!.Price, returnAction, service.ServiceName);
                    return ServiceResult<string>.Success(result);
                case PaymentMethod.ZaloPay:
                    result = await _zaloPay.CreateZaloPayOrder((int)service!.Price, returnAction, service.ServiceName);
                    return ServiceResult<string>.Success(result);
            }

            return ServiceResult<string>.Failed(ServiceError.ValidationFailed("Unsupported payment type"));
        }

        public async Task<ICollection<Booking>> GetAppointments(Guid userId, string status) => await _unitOfWork.Bookings.GetAppointments(userId, status);

        

        public async Task<ServiceResult<ICollection<Booking>>> GetAllUserBooking(Guid userId)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId, x => x
            .Include(b => b.BookingNavigation)
            .ThenInclude(b => b.TherapistNavigation)
            .ThenInclude(b => b.UserNavigation)
            .Include(b => b.BookingNavigation)
            .ThenInclude(b => b.ServiceNavigation));

            if (user == null)
            {
                return ServiceResult<ICollection<Booking>>.Failed(ServiceError.NotExisted("Can not find any matching user with the provided id"));
            }

            return ServiceResult<ICollection<Booking>>.Success(user.BookingNavigation);
        }

        public async Task<ServiceResult<Booking>> GetBookingInformation(Guid bookingId)
        {

            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(bookingId, x => x
            .Include(x => x.ServiceNavigation)
            .Include(x => x.TherapistNavigation)
            .ThenInclude(x => x.UserNavigation)
            .Include(x => x.ScheduleNavigation)
            .ThenInclude(x => x.ServiceDetailNavigation));

            if (booking == null)
            {
                return ServiceResult<Booking>.Failed(ServiceError.NotExisted("Can not find any booking information with the provided id"));
            }

            return ServiceResult<Booking>.Success(booking);
        }

        public Task<ServiceResult<Booking>> UpdateBookingInformation(string id, Booking bookingInformation)
        {
            throw new NotImplementedException();
        }

        public Task<(Booking, Service)> UpdateBookingService(Guid bookingId, DateTime dateTime)
        {
            throw new NotImplementedException();
        }

    }
}
