using Castle.Core.Resource;
using Entities;
using Microsoft.EntityFrameworkCore;
using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
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
        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<(Booking, Service)> CreateNewBooking(Guid customerId, Guid serviceId, DateTime date )
        {
            DateTime date = dateTime.Date;
            TimeSpan time = dateTime.TimeOfDay;

            var service = await _unitOfWork.Repository<Service>()
            .GetByConditionAsync(s => s.Id == serviceId,
          query => query.Include(s => s.ServiceDetailNavigation)
                        .Include(s => s.ServiceImageNavigation));


            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                ServiceId = serviceId,
                ReservedTime = dateTime,
                Status = BookingStatus.NotStarted,
                TotalPrice = service.Price,

            };
            await _unitOfWork.Repository<Booking>().AddAsync(booking);
            var serviceDetails = service.ServiceDetailNavigation
                                     .Where(sd => !sd.IsDetele)
                                     .OrderBy(sd => sd.Step)
                                     .ToList();
            if (serviceDetails == null)
            {
                var schedule = new Schedule
                {
                    BookingId = booking.Id,
                    Date = date,
                    ReservedStartTime = time,
                    ReservedEndTime = time.Add(TimeSpan.FromMinutes(Duration)) // Thời gian kết thúc
                };
            }
            if (service.ServiceDetailNavigation != null)
            {
                foreach (var serviceDetail in serviceDetails)
                {
                    var schedule = new Schedule
                    {
                        BookingId = booking.Id,
                        ServiceDetailId = serviceDetail.Id,
                        Date = date,
                        ReservedStartTime = time,
                        ReservedEndTime = time.Add(TimeSpan.FromMinutes(serviceDetail.Duration))  // Thời gian kết thúc
                    };

                    await _unitOfWork.Repository<Schedule>().AddAsync(schedule);
                    await _unitOfWork.Complete();

                    date = date.AddDays(serviceDetail.DateToNextStep);
                }

            }
        }

        public Task<ServiceResult<ICollection<Booking>>> GetAllUserBooking(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<(User?, List<Booking>)> GetAppointments(Guid customerId, BookingStatus status)
        {
            var bookings = (await _unitOfWork.Repository<Booking>().ListAsync(
                b => b.CustomerId == customerId && b.Status == status,
                includeProperties: query => query.Include(b => b.ServiceNavigation)
                                                 .ThenInclude(s => s.ServiceDetailNavigation)
            )).ToList();

            return (await _unitOfWork.Repository<User>().GetEntityByIdAsync(customerId), bookings);
        }




        public Task<ServiceResult<Booking>> GetBookingInformation(Guid bookingId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<Booking>> UpdateBookingInformation(Guid id, Booking bookingInformation)
        {
            throw new NotImplementedException();
        }

        public Task<(Booking, Service)> UpdateBookingService(Guid bookingId, DateTime dateTime)
        {
            throw new NotImplementedException();// CÓ DÂU CHẤM HỎI RẤT LỚN VỀ NỘI RÁNGF BUỘC... 
        }
    }
}
