using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.BookingService
{
    public class BookingService : IBookingService
    {
        public Task<ServiceResult<Booking>> CreateNewBooking(Booking bookingInformation)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<ICollection<Booking>>> GetAllUserBooking(Guid userId)
        {
            throw new NotImplementedException();
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
