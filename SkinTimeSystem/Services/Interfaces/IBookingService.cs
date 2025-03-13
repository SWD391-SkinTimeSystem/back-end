using BusinessObject.Entities;
using Services.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookingService
    {
        Task<ICollection<Booking>> GetAppointments(Guid userId, string status);

        Task<ServiceResult<Booking>> GetBookingInformation(Guid bookingId);

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
        Task<ServiceResult<string>> CreateNewBooking(Booking booking, string returnAction, string returnURL, string failureURL, TimeOnly serviceHour, string paymentMethod, Guid userId);

        Task<ServiceResult<Booking>> UpdateBookingInformation(string id, Booking bookingInformation);
    }
}
