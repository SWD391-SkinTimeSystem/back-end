using Cursus.Core.Options.PaymentSetting;
using Microsoft.EntityFrameworkCore;
using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.DAL.Enum.Schedule;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly VNPay _vnPay;
        private readonly ZaloPay _zaloPay;
        public BookingService(IUnitOfWork unitOfWork, VNPay vNPay, ZaloPay zaloPay)
        {
            _unitOfWork = unitOfWork;
            _vnPay = vNPay;
            _zaloPay = zaloPay;
        }

        public async Task<string> CreateNewBooking(string returnCallBack, Guid serviceId, string bank)
        {
            var service = _unitOfWork.Repository<Service>().GetById(serviceId);

            if (!Enum.TryParse(bank, true, out PaymentMethod paymentMethod) || !Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
            {
                throw new InvalidOperationException("Invalid payment method.");
            }
            return paymentMethod switch
            {
                PaymentMethod.VnPay => await _vnPay.CreateVNPayOrder((int)service.Price, returnCallBack, service.ServiceName),
                PaymentMethod.ZaloPay => await _zaloPay.CreateZaloPayOrder((int)service.Price, returnCallBack, service.ServiceName),
                _ => throw new InvalidOperationException("Unsupported payment bank."),
            };
        }

        public async Task<ICollection<Booking>> GetAppointments(Guid userId, string status)
        {
            var bookingStatus = Enum.Parse<BookingStatus>(status);

            var bookings = await _unitOfWork.Repository<Booking>().ListAsync(
        filter: b => b.CustomerId == userId && b.Status == bookingStatus,
        includeProperties: q => q
            .Include(b => b.ServiceNavigation)
            .ThenInclude(s => s.ServiceDetailNavigation)
            .Include(b => b.TherapistNavigation)
            .ThenInclude(t => t.UserNavigation)
            .Include(b => b.ScheduleNavigation)
    );


            return bookings.ToList();

        }

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
            throw new NotImplementedException();// CÓ DÂU CHẤM HỎI RẤT LỚN VỀ NỘI RÁNGF BUỘC... 
        }

    }
}
