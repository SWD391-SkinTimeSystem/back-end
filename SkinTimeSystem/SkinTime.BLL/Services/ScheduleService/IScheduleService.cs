using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.ScheduleService
{
    public interface IScheduleService
    {

        Task<ServiceResult<Schedule>> CreateSchedule(Schedule schedule);
        Task<ServiceResult<Schedule>> GetSchedule(Guid scheduleId);
        Task<ServiceResult<ICollection<Schedule>>> GetAllSchedules();
        Task<ServiceResult<ICollection<Schedule>>> GetAllSchedules(Expression<Func<Schedule,bool>> expression);
        Task<ServiceResult<Schedule>> UpdateSchedule(Guid id, Schedule schedule);
        Task<ServiceResult<Schedule>> DeleteSchedule(Guid id);

        /// <summary>
        ///     <para>
        ///         Generate a new schedule based on the booking id and the previous schedule. This is used for view purposes only.
        ///     </para>
        ///     <para>
        ///         <b>Note:</b> Using this method does not save the created schedule to database!
        ///     </para>
        /// </summary>
        /// <param name="bookingId">The id of the booking</param>
        /// <returns>
        ///     <para>
        ///         A <see cref="ServiceResult{T}.Success"/> with the new schedule data generated.
        ///     </para>
        ///     <para>
        ///         A <see cref="ServiceResult{T}.Failed"/> with error type of <see cref="ServiceError.NotExisted(string)"/> if the booking does not exist.
        ///     </para>
        ///     <para>
        ///         A <see cref="ServiceResult{T}.Failed"/> with error type of <see cref="ServiceError.ValidationFailed(string)"/> if the booking canceld or finished.
        ///     </para>
        /// </returns>
        Task<ServiceResult<Schedule>> GenerateScheduleForBooking(string bookingId);
        Task<ServiceResult<ICollection<Schedule>>> GetUserSchedules(Guid userId);
        Task<ServiceResult<ICollection<Schedule>>> GetBookingSchedule(Guid bookingId);
    }
}
