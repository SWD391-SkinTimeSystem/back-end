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
        Task<ServiceResult<ICollection<Booking>>> GetAllUserBooking(string userId);

        Task<ServiceResult<Booking>> GetBookingInformation(string bookingId);

        /// <summary>
        ///     <pra>
        ///         Create a new booking reservation for the given service.
        ///     </pra>
        ///     <para>
        ///         <b>Note:</b> Create a new booking also will create a schedule record for the first service step by default.
        ///     </para>
        /// </summary>
        /// <param name="bookingInformation"></param>
        /// <returns>The service result represent operation result. The data will be the newly created booking entity.</returns>
        Task<ServiceResult<Booking>> CreateNewBooking(Booking bookingInformation);

        Task<ServiceResult<Booking>> UpdateBookingInformation(string id, Booking bookingInformation);
    }
}
