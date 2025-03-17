using BusinessObject.Entities;
using BusinessObject.Enum;
using BusinessObject.Schedule;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> CreateBookingAndSchedule(Booking booking, TimeOnly serviceHour)
        {
            var service = await _context.Services
                .Include(s => s.ServiceDetailNavigation)
                .FirstOrDefaultAsync(se => se.Id == booking.ServiceId);

            if (service == null)
            {
                return false; 
            }

            booking.Id = Guid.NewGuid();
            booking.TotalPrice = service.Price;

            await _context.Bookings.AddAsync(booking);

            var firstStepServiceDetail = service.ServiceDetailNavigation
                .Where(sd => !sd.IsDetele)
                .OrderBy(sd => sd.Step)
                .FirstOrDefault();

            if (firstStepServiceDetail == null)
            {
                return false;
            }

            var newSchedule = new Schedule
            {
                Id = Guid.NewGuid(),
                BookingId = booking.Id,
                ServiceDetailId = firstStepServiceDetail.Id,
                Status = ScheduleStatus.NotStarted,
                ReservedStartTime = serviceHour,
                ReservedEndTime = TimeOnly.FromTimeSpan(
                    serviceHour.ToTimeSpan().Add(TimeSpan.FromMinutes(firstStepServiceDetail.Duration))
                ),
                Date = DateOnly.FromDateTime(booking.ReservedTime)
            };

            await _context.Schedules.AddAsync(newSchedule);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Booking?> GetBookingInformation(Guid bookingId)
        {
            return await _context.Bookings
                .Include(x => x.ServiceNavigation)
                .Include(x => x.TherapistNavigation)
                    .ThenInclude(x => x.UserNavigation)
                .Include(x => x.ScheduleNavigation)
                    .ThenInclude(x => x.ServiceDetailNavigation)
                .SingleOrDefaultAsync(x => x.Id == bookingId);
        }


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
