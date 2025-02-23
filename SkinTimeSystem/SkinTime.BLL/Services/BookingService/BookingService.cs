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

        public async Task<ServiceResult<ICollection<Booking>>> GetAllUserBooking(string userId)
        {
            Guid parsedId;

            if (!Guid.TryParse(userId, out parsedId))
            {
                return ServiceResult<ICollection<Booking>>.Failed(ServiceError.ValidationFailed("Validation failed!", new() { {"user_id", "Incorrect user id format" } }));
            }

            // Get the user information with provided user_id,
            // This is used for both validating user existance and getting the booking information.
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(parsedId, x => x
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

        public async Task<ServiceResult<Booking>> GetBookingInformation(string bookingId)
        {
            Guid parsedId;

            if (!Guid.TryParse(bookingId, out parsedId))
            {
                return ServiceResult<Booking>.Failed(ServiceError.ValidationFailed("Validation failed!", new() { 
                    { "booking_id", "Incorrect booking id format" } 
                }));
            }

            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(parsedId, x => x
            .Include(x => x.ServiceNavigation)
            .Include(x => x.TransactionNavigation)
            .Include(x => x.FeedbackNavigation!)
            .Include(x => x.CustomerNavigation)
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
