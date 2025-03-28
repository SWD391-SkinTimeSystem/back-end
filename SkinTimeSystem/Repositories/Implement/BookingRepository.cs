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

        public async Task<bool> CreateBookingAndSchedule(Booking booking, TimeOnly serviceHour, Guid key)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(se => se.Id == booking.ServiceId);
            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(se => se.Id == booking.VoucherId);
            decimal discount = voucher?.Discount ?? 0;
            booking.Id = Guid.NewGuid();
            booking.TransactionId = key;
            booking.TotalPrice = service.Price*(1m - discount / 100);

            await _context.Bookings.AddAsync(booking);

            var serviceDetails = service.ServiceDetailNavigation
               .Where(sd => !sd.IsDetele)
               .OrderBy(sd => sd.Step)
               .ToList();

            List<Schedule> schedules = new List<Schedule>();

            TimeOnly defaultTime = new TimeOnly(0, 0); 
            DateOnly currentDate = DateOnly.FromDateTime(booking.ReservedTime); 

            for (int i = 0; i < serviceDetails.Count; i++)
            {
                var step = serviceDetails[i];

                var schedule = new Schedule
                {
                    Id = Guid.NewGuid(),
                    BookingId = booking.Id,
                    ServiceDetailId = step.Id,
                    Status = ScheduleStatus.NotStarted,
                    ReservedStartTime = i == 0 ? serviceHour : defaultTime, 
                    ReservedEndTime = i == 0
                        ? TimeOnly.FromTimeSpan(serviceHour.ToTimeSpan().Add(TimeSpan.FromMinutes(step.Duration)))
                        : defaultTime, 
                    Date = currentDate 
                };

                schedules.Add(schedule);
                if (i < serviceDetails.Count - 1)
                {
                    currentDate = currentDate.AddDays(step.DateToNextStep);
                }
            }

            await _context.Schedules.AddRangeAsync(schedules);
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
        public async Task<ICollection<Booking>> GetAppointmentsOfTherapist(Guid userId, string status)
        {
            var bookingStatus = Enum.Parse<BookingStatus>(status);
            return await _context.Bookings
                .Where(b => b.TherapistId == userId && b.Status == bookingStatus)
                .Include(b => b.ServiceNavigation)
                    .ThenInclude(s => s.ServiceDetailNavigation)
                .Include(b => b.TherapistNavigation)
                    .ThenInclude(t => t.UserNavigation)
                .Include(b => b.ScheduleNavigation)
                .ToListAsync();
        }

        public async Task<PaginationResult<Booking>> GetBookingPaginated(int page, int pageSize)
        {
            return await AsPaginated(page, pageSize, null, includes: x => x.Include(x => x.TherapistNavigation).Include(x=>x.ServiceNavigation).Include(X=> X.ScheduleNavigation).Include(X => X.CustomerNavigation),
                order: x => x.OrderBy(x => x.CreatedTime));
        }
    }
}
