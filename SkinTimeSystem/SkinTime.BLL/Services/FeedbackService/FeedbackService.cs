using Microsoft.EntityFrameworkCore;
using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.FeedbackService
{
    public class FeedbackService : IFeedbackService
    {

        private readonly IUnitOfWork _unitOfWork;

        public FeedbackService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<Feedback>> CreateNewFeedback(Feedback feedback)
        {
            var booking = await _unitOfWork.Repository<Booking>()
                .GetByIdAsync(feedback.BookingId, x => x.Include(b => b.FeedbackNavigation));

            if (booking == null)
            {
                return ServiceResult<Feedback>
                    .Failed(ServiceError.NotExisted("The booking information not found for the given id"));
            }

            if (booking.Status != DAL.Enum.BookingStatus.Canceled || booking.Status != DAL.Enum.BookingStatus.Completed)
            {
                return ServiceResult<Feedback>
                    .Failed(ServiceError.NotExisted("The booking information not found for the given id"));
            }

            if (booking.FeedbackNavigation != null)
            {
                return ServiceResult<Feedback>
                    .Failed(ServiceError.Existed("Feedback already existed, you can only delete or update."));
            }

            booking.FeedbackNavigation = feedback;
            _unitOfWork.Repository<Booking>().Update(booking);
            await _unitOfWork.Complete();

            return ServiceResult<Feedback>.Success(booking.FeedbackNavigation);
        }

        public async Task<ServiceResult<ICollection<Feedback>>> GetAllServiceFeedback(string serviceId)
        {
            if(Guid.TryParse(serviceId, out var parsedId))
            {
                IEnumerable<Booking> results = (await _unitOfWork.Repository<Booking>()
                    .ListAsync(x => 
                    x.Include(b => b.FeedbackNavigation)
                    .Include(b => b.CustomerNavigation)
                    .Include(b => b.ServiceNavigation), 
                        x => x.ServiceId == parsedId && x.FeedbackNavigation != null))
                    .OrderBy(x => x.FeedbackNavigation!.CreatedTime);
                    
                return ServiceResult<ICollection<Feedback>>
                    .Success(results.Select(x => x.FeedbackNavigation!).ToList());
            }

            return ServiceResult<ICollection<Feedback>>
                .Failed(ServiceError.ValidationFailed("The service id is not in the correct format"));
        }

        public async Task<ServiceResult<ICollection<Feedback>>> GetAllTherapistFeedback(string therapistId)
        {
            if (Guid.TryParse(therapistId, out var parsedId))
            {
                IEnumerable<Booking> results = (await _unitOfWork.Repository<Booking>()
                    .ListAsync(x => x.Include(b => b.FeedbackNavigation)
                    .Include(b => b.CustomerNavigation)
                    .Include(b => b.ServiceNavigation),
                        x => x.TherapistId == parsedId && x.FeedbackNavigation != null))
                    .OrderBy(x => x.FeedbackNavigation!.CreatedTime);

                return ServiceResult<ICollection<Feedback>>
                    .Success(results.Select(x => x.FeedbackNavigation!).ToList());
            }

            return ServiceResult<ICollection<Feedback>>
                .Failed(ServiceError.ValidationFailed("The therapist id is not in the correct format"));
        }

        public async Task<ServiceResult<ICollection<Feedback>>> GetAllUserFeedback(string userId)
        {
            if (Guid.TryParse(userId, out var parsedId))
            {
                IEnumerable<Booking> results = (await _unitOfWork.Repository<Booking>()
                    .ListAsync(x => x.Include(b => b.FeedbackNavigation),
                        x => x.CustomerId == parsedId && x.FeedbackNavigation != null))
                    .OrderBy(x => x.FeedbackNavigation!.CreatedTime);

                return ServiceResult<ICollection<Feedback>>
                    .Success(results.Select(x => x.FeedbackNavigation!).ToList());
            }

            return ServiceResult<ICollection<Feedback>>
                .Failed(ServiceError.ValidationFailed("The user id is not in the correct format"));
        }

        public async Task<ServiceResult<ICollection<Feedback>>> GetAllFeedback()
        {
            IEnumerable<Booking> results = await _unitOfWork.Repository<Booking>()
                    .ListAsync(x => 
                    x.Include(b => b.FeedbackNavigation)
                    .Include(b => b.ServiceNavigation)
                    .Include(b => b.CustomerNavigation)
                    .Include(b => b.TherapistNavigation)
                    .ThenInclude(b => b.UserNavigation),
                    x => x.FeedbackNavigation != null);

            return ServiceResult<ICollection<Feedback>>
                .Success(results.Select(x => x.FeedbackNavigation!).ToList());
        }

        public async Task<ServiceResult<Feedback>> GetFeedbackWithId(string feedbackId)
        {
            if (Guid.TryParse(feedbackId, out var parsedId))
            {
                var result = await _unitOfWork.Repository<Feedback>().GetByIdAsync(parsedId, x=>
                x.Include(x => x.BookingNavigation)
                .Include(x => x.BookingNavigation).ThenInclude(x => x.ServiceNavigation)
                .Include(x => x.BookingNavigation).ThenInclude(x => x.CustomerNavigation)
                .Include(x => x.BookingNavigation).ThenInclude(x => x.TherapistNavigation)
                .Include(x => x.BookingNavigation).ThenInclude(x => x.TherapistNavigation)
                .ThenInclude(x => x.UserNavigation));

                if (result == null)
                {
                    return ServiceResult<Feedback>
                        .Failed(ServiceError.NotFound("Can not find the feedback with required information"));
                }

                return ServiceResult<Feedback>.Success(result);
            }

            return ServiceResult<Feedback>
                .Failed(ServiceError.ValidationFailed("The feedback id is not in the correct format"));
        }

        public async Task<ServiceResult<Feedback>> GetBookingFeedback(string bookingId)
        {
            if (Guid.TryParse(bookingId, out var parsedId))
            {
                var result = await _unitOfWork.Repository<Feedback>()
                    .GetByConditionAsync(x => x.BookingId == parsedId, 
                    x => x.Include(x => x.BookingNavigation)
                    .Include(x => x.BookingNavigation).ThenInclude(x => x.ServiceNavigation)
                    .Include(x => x.BookingNavigation).ThenInclude(x => x.CustomerNavigation)
                    .Include(x => x.BookingNavigation).ThenInclude(x => x.TherapistNavigation)
                    .Include(x => x.BookingNavigation).ThenInclude(x => x.TherapistNavigation)
                    .ThenInclude(x => x.UserNavigation));

                if (result == null)
                {
                    return ServiceResult<Feedback>
                        .Failed(ServiceError.NotExisted("Can not find the booking with required information"));
                }

                return ServiceResult<Feedback>.Success(result);
            }

            return ServiceResult<Feedback>
                .Failed(ServiceError.ValidationFailed("The booking id is not in the correct format"));
        }
    }
}
