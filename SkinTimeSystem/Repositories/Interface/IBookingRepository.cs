using BusinessObject.Entities;

namespace Repositories.Interface
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<ICollection<Booking>> GetAppointments(Guid userId, string status);
        Task<ICollection<Booking>> GetAppointmentsOfTherapist(Guid userId, string status);
        Task<bool> CreateBookingAndSchedule(Booking booking, TimeOnly serviceHour,Guid key);
        Task<Booking> GetBookingInformation(Guid bookingId);
    }
}
