using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.BookingService
{
    public interface IBookingService
    {
        Task<ServiceResult<ICollection<Booking>>> GetAllUserBooking(Guid userId);

        Task<ServiceResult<Booking>> GetBookingInformation(Guid bookingId);

        Task<(Booking, Service)> CreateNewBooking(Guid customerId,Guid serviceId, DateTime dateTime);
        Task<(Booking, Service)> UpdateBookingService(Guid bookingId, DateTime dateTime);
        
        Task<ServiceResult<Booking>> UpdateBookingInformation(Guid id, Booking bookingInformation);
        Task<(User, List<Booking>)> GetAppointments(Guid customerId, BookingStatus status);
    }
}
