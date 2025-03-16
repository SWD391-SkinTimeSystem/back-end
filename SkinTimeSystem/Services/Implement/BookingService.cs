using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enum;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories.Interface;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.Booking;
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
        private readonly IMapper _mapper;
        private readonly VNPay _vnPay;
        private readonly ZaloPay _zaloPay;
        private readonly ICache _cache;
        public BookingService(IMapper mapper,ICache cache,IUnitOfWork unitOfWork, VNPay vNPay, ZaloPay zaloPay)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _vnPay = vNPay;
            _zaloPay = zaloPay;
            _cache = cache;
        }

        public async Task<ServiceResult<string>> CreateBooking(BookingServiceDTO booking, Guid userId, string? returnAction)
        {
            var service = _unitOfWork.Repository<Service>().GetById(booking.ServiceId);
            var bookingWithId = booking as BookingServiceWithIdDTO ?? new BookingServiceWithIdDTO(booking, userId);
            var bank = Enum.TryParse(booking.PaymentMethod, true, out PaymentMethod pm) && Enum.IsDefined(pm) ? pm : (PaymentMethod?)null;
            string redisKey = $"{Guid.NewGuid()}";
            await _cache.SetAsync(redisKey, JsonConvert.SerializeObject(bookingWithId), TimeSpan.FromMinutes(30));
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

        public async Task<ICollection<BokingServiceStatusDTO>> GetAppointments(Guid userId, string status)
        {

            var listBooking = await _unitOfWork.Bookings.GetAppointments(userId, status);
            return _mapper.Map<ICollection<BokingServiceStatusDTO>>(listBooking);

        }


        public async Task<ServiceResult<BookingDetailDTO>> GetBookingInformation(Guid bookingId)
        {


            var booking = _unitOfWork.Bookings.GetBookingInformation(bookingId);

            if (booking == null)
            {
                return ServiceResult<BookingDetailDTO>.Failed(ServiceError.NotExisted("Can not find any booking information with the provided id"));
            }
            var bookingDTO = _mapper.Map<BookingDetailDTO>(booking);
            return ServiceResult<BookingDetailDTO>.Success(bookingDTO);
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
