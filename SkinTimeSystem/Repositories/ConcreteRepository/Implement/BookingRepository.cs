using BusinessObject.Entities;
using BusinessObject.Enum;
using DAOs.Data;
using Microsoft.EntityFrameworkCore;
using Repositories.ConcreteRepository.Interface;
using Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ConcreteRepository.Implement
{
    public class BookingRepository : GenericRepository<Booking> , IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context):base(context) { }

        public async Task<ICollection<Booking>> GetAppointments(Guid userId, string status)
        {
            var bookingStatus = Enum.Parse<BookingStatus>(status);
            return await _context.Bookings
                .Where(b => b.CustomerId == userId && b.Status == bookingStatus)
                .Include(b => b.ServiceNavigation)
                    .ThenInclude(s => s.ServiceDetailNavigation)
                .Include(b => b.TherapistNavigation)
                    .ThenInclude(t => t.UserNavigation)
                .Include(b => b.ScheduleNavigation)
                .ToListAsync();
        }
    }
}
