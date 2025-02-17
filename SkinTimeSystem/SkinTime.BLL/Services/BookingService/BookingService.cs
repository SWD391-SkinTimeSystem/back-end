using Castle.Core.Resource;
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
        public async Task<(Booking, Service)> CreateNewBooking(Guid serviceId, DateTime dateTime)
        {
            var service = await _unitOfWork.Repository<Service>().GetEntityByIdAsync(serviceId);

            // Kiểm tra nếu Service không tồn tại
            if (service == null)
            {
                return (null, null);
            }

            // Nếu Service không có ServiceDetails, thêm Schedule với ngày hiện tại
            if (service.ServiceDetailNavigation == null || !service.ServiceDetailNavigation.Any())
            {
                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    ServiceId = serviceId,
                    ReservedTime = dateTime,
                    Status = BookingStatus.NotStarted
                };

                try
                {
                    // Thêm booking vào database
                    await _unitOfWork.Repository<Booking>().AddAsync(booking);
                    await _unitOfWork.Complete();

                    // Thêm Schedule với ngày hiện tại
                    var schedule = new Schedule
                    {
                        BookingId = booking.Id,
                        ServiceDetailId = Guid.NewGuid(),  
                        Date = dateTime, // Lấy ngày hiện tại cho lịch trình
                        ReservedStartTime = dateTime,
                        ReservedEndTime = dateTime.AddMinutes(60)  // Giả sử duration là 60 phút
                    };

                    await _unitOfWork.Repository<Schedule>().AddAsync(schedule);
                    await _unitOfWork.Complete();

                    return (booking, service);
                }
                catch (Exception ex)
                {
                    return (null, null);
                }
            }

            // Nếu Service có ServiceDetail, tạo Schedule theo các bước
            var serviceDetails = service.ServiceDetailNavigation
                                        .Where(sd => !sd.IsDetele) // Loại bỏ những ServiceDetail đã bị xóa
                                        .OrderBy(sd => sd.Step)  // Sắp xếp theo bước
                                        .ToList();

            if (!serviceDetails.Any())
            {
                return (null, null);
            }

            var bookingWithService = new Booking
            {
                Id = Guid.NewGuid(),
                ServiceId = serviceId,
                ReservedTime = dateTime,
                Status = BookingStatus.NotStarted
            };

            try
            {
                // Thêm booking vào database
                await _unitOfWork.Repository<Booking>().AddAsync(bookingWithService);
                await _unitOfWork.Complete();

                // Tính toán lịch trình bắt đầu với Step 1
                DateTime currentDateTime = dateTime;

                foreach (var serviceDetail in serviceDetails)
                {
                    var schedule = new Schedule
                    {
                        BookingId = bookingWithService.Id,
                        ServiceDetailId = serviceDetail.Id,
                        Date = currentDateTime, // Ngày hiện tại của lịch trình
                        ReservedStartTime = currentDateTime, // Thời gian bắt đầu
                        ReservedEndTime = currentDateTime.AddMinutes(serviceDetail.Duration) // Thời gian kết thúc
                    };

                    await _unitOfWork.Repository<Schedule>().AddAsync(schedule);
                    await _unitOfWork.Complete();

                    currentDateTime = currentDateTime.AddDays(serviceDetail.DateToNextStep);
                }

                return (bookingWithService, service);
            }
            catch (Exception ex)
            {
                return (null, null);
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



    }
}
