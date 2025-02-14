using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
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

        Task<ServiceResult<Booking>> CreateNewBooking(Booking bookingInformation);

        Task<ServiceResult<Booking>> UpdateBookingInformation(Guid id, Booking bookingInformation);
    }
}
