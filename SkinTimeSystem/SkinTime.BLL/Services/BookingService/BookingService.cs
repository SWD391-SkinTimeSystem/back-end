using Microsoft.EntityFrameworkCore;
using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
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

        public Task<ServiceResult<Booking>> CreateNewBooking(Booking bookingInformation)
        {
            throw new NotImplementedException();
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
    }
}
