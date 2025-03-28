using BusinessObject.Entities;
using Repositories;
using Services.Commons;
using Services.Commons.DTOs.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookingService
    {
        Task<ICollection<BokingServiceStatusDTO>> GetAppointments(Guid userId, string status);
        Task<ICollection<BokingServiceStatusDTO>> GetAppointmentsOfTherapist(Guid userId, string status);
        Task<ServiceResult<PaginationResult<BookingAll>>> GetAllBooking(int page, int pageSize);
        Task<ServiceResult<BookingDetailDTO>> GetBookingInformation(Guid bookingId);
        Task<ServiceResult<string>> CreateBooking(BookingServiceDTO booking, Guid userId,string? returnAction);

        Task<ServiceResult<Booking>> UpdateBookingInformation(string id, Booking bookingInformation);
        Task<Guid?> GetTransaction(Guid idBooking);

    }
}
