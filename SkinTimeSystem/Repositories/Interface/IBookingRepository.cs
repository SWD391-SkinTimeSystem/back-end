using BusinessObject.Entities;
using System.Linq.Expressions;

namespace Repositories.Interface
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<ICollection<Booking>> GetAppointments(Guid userId, string status);
        Task<ICollection<Booking>> GetAppointmentsOfTherapist(Guid userId, string status);
        Task<bool> CreateBookingAndSchedule(Booking booking, TimeOnly serviceHour,Guid key);
        Task<Booking> GetBookingInformation(Guid bookingId);
        Task<PaginationResult<Booking>> GetBookingPaginated(int page, int pageSize);
    }
}
