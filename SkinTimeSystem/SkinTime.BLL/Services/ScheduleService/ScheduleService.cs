using Microsoft.EntityFrameworkCore;
using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.DAL.Enum.Schedule;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SkinTime.BLL.Services.ScheduleService
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<Schedule>> CreateSchedule(Schedule schedule)
        {
            try
            {
                await _unitOfWork.Repository<Schedule>().AddAsync(schedule);
                await _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\nInnerException:{ex.InnerException?.Message}\n{ex.GetType()}:{ex.StackTrace}");
                return ServiceResult<Schedule>.Failed(ServiceError.UnhandledException(ex.Message));
            }

            return ServiceResult<Schedule>.Success(null);
        }

        public Task<ServiceResult<Schedule>> DeleteSchedule(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<Schedule>> GenerateScheduleForBooking(string bookingId)
        {
            Guid parsedId;

            if (!Guid.TryParse(bookingId, out parsedId))
            {
                Dictionary<object, object> error = new() { { "id", "The given id have incorrect format." } };
                return ServiceResult<Schedule>.Failed(ServiceError.ValidationFailed("Validation errors occured!", error));
            }

            // Get the booking information
            var booking = await _unitOfWork.Repository<Booking>()
                .GetByIdAsync(parsedId, x => x
                .Include(x => x.ServiceNavigation)
                .Include(x => x.ScheduleNavigation)
                .ThenInclude(x => x.ServiceDetailNavigation));

            if (booking == null)
            {
                return ServiceResult<Schedule>.Failed(ServiceError.NotExisted("No booking found for the provided id"));
            }

            if (booking.Status == BookingStatus.Canceled || booking.Status == BookingStatus.NotStarted || booking.ScheduleNavigation.Count == 0)
            {
                return ServiceResult<Schedule>.Failed(ServiceError.ValidationFailed("This booking is invalid for a new schedule creation!"));
            }

            // Get the last schedule from the previous work time.
            Schedule previousSchedule = booking.ScheduleNavigation.Last();

            // Get and validate the next step detail
            var nextStepService = await _unitOfWork.Repository<ServiceDetail>().FindAsync(x => x.ServiceID == booking.ServiceId && x.Step == previousSchedule.ServiceDetailNavigation.Step + 1);

            if (nextStepService == null)
            {
                return ServiceResult<Schedule>.Failed(ServiceError.ValidationFailed("This booking has already completed all service step!"));
            }

            Schedule schedule = new()
            {
                ServiceDetailId = nextStepService.ServiceID,
                BookingId = parsedId,
                Status = ScheduleStatus.NotStarted,
                Date = previousSchedule.Date.AddDays(previousSchedule.ServiceDetailNavigation.DateToNextStep),
                ReservedStartTime = previousSchedule.ReservedStartTime,
                ReservedEndTime = previousSchedule.ReservedEndTime,
            };

            return ServiceResult<Schedule>.Success(schedule);
        }

        public async Task<ServiceResult<ICollection<Schedule>>> GetAllSchedules()
        {
            IEnumerable<Schedule> result = await _unitOfWork.Repository<Schedule>().ListAsync();

            return ServiceResult<ICollection<Schedule>>.Success(result.ToList());
        }

        public async Task<ServiceResult<ICollection<Schedule>>> GetAllSchedules(Expression<Func<Schedule, bool>> expression)
        {
            IEnumerable<Schedule> result = await _unitOfWork.Repository<Schedule>().ListAsync(expression);

            return ServiceResult<ICollection<Schedule>>.Success(result.ToList());
        }

        public async Task<ServiceResult<ICollection<Schedule>>> GetBookingSchedule(Guid bookingId)
        {
            IEnumerable<Schedule> result = await _unitOfWork.Repository<Schedule>().ListAsync(x => x.Id == bookingId);

            return ServiceResult<ICollection<Schedule>>.Success(result.ToList());
        }

        public async Task<ServiceResult<Schedule>> GetSchedule(Guid scheduleId)
        {
            var schedule = await _unitOfWork.Repository<Schedule>().GetByIdAsync(scheduleId);

            if (schedule != null)
            {
                return ServiceResult<Schedule>.Success(schedule);
            }

            return ServiceResult<Schedule>.Failed(ServiceError.NotFound($"Can not find schedule entity with id {scheduleId}"));
        }

        public async Task<ServiceResult<ICollection<Schedule>>> GetUserSchedules(Guid userId)
        {
            // Check if the user exist or not
            if (!_unitOfWork.Repository<User>().Exists(userId))
            {
                return ServiceResult<ICollection<Schedule>>.Failed(ServiceError.NotExisted($"User does not exist for id {userId}"));
            }

            var result = await _unitOfWork.Repository<Schedule>()
                .ListAsync(x => x.BookingNavigation.CustomerId == userId || x.BookingNavigation.TherapistId == userId);

            return ServiceResult<ICollection<Schedule>>.Success(result.ToList());
        }

        public Task<ServiceResult<Schedule>> UpdateSchedule(Guid id, Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}
