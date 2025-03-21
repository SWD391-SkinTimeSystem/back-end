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
            booking.Status = BookingStatus.Doing;
            booking.TotalPrice = service.Price;

            await _context.Bookings.AddAsync(booking);

            //var firstStepServiceDetail = service.ServiceDetailNavigation
            //    .Where(sd => !sd.IsDetele)
            //    .OrderBy(sd => sd.Step)
            //    .FirstOrDefault();

            //if (firstStepServiceDetail == null)
            //{
            //    return false;
            //}

            //var newSchedule = new Schedule
            //{
            //    Id = Guid.NewGuid(),
            //    BookingId = booking.Id,
            //    ServiceDetailId = firstStepServiceDetail.Id,
            //    Status = ScheduleStatus.NotStarted,
            //    ReservedStartTime = serviceHour,
            //    ReservedEndTime = TimeOnly.FromTimeSpan(
            //        serviceHour.ToTimeSpan().Add(TimeSpan.FromMinutes(firstStepServiceDetail.Duration))
            //    ),
            //    Date = DateOnly.FromDateTime(booking.ReservedTime)
            //};


            var serviceDetails = service.ServiceDetailNavigation
               .Where(sd => !sd.IsDetele)
               .OrderBy(sd => sd.Step)
               .ToList();

            if (!serviceDetails.Any())
            {
                return false;
            }

            List<Schedule> schedules = new List<Schedule>();

            //TimeOnly defaultTime = new TimeOnly(0, 0); // Giờ mặc định
            //DateOnly defaultDate = new DateOnly(1,1,1); // Sử dụng null thay vì DateOnly(0, 0, 0)

            //for (int i = 0; i < serviceDetails.Count; i++)
            //{
            //    var step = serviceDetails[i];

            //    var schedule = new Schedule
            //    {
            //        Id = Guid.NewGuid(),
            //        BookingId = booking.Id,
            //        ServiceDetailId = step.Id,
            //        Status = ScheduleStatus.NotStarted,
            //        ReservedStartTime = i == 0 ? serviceHour : defaultTime, // Step đầu có giờ, các step sau dùng defaultTime
            //        ReservedEndTime = i == 0
            //            ? TimeOnly.FromTimeSpan(serviceHour.ToTimeSpan().Add(TimeSpan.FromMinutes(step.Duration)))
            //            : defaultTime, // Step sau mặc định = 00:00
            //        Date = i == 0 ? DateOnly.FromDateTime(booking.ReservedTime) : defaultDate // Để null nếu không phải step đầu
            //    };

            //    schedules.Add(schedule);
            //}

            TimeOnly defaultTime = new TimeOnly(0, 0); // Giờ mặc định
            DateOnly currentDate = DateOnly.FromDateTime(booking.ReservedTime); // Ngày step đầu tiên

            for (int i = 0; i < serviceDetails.Count; i++)
            {
                var step = serviceDetails[i];

                var schedule = new Schedule
                {
                    Id = Guid.NewGuid(),
                    BookingId = booking.Id,
                    ServiceDetailId = step.Id,
                    Status = ScheduleStatus.NotStarted,
                    ReservedStartTime = i == 0 ? serviceHour : defaultTime, // Chỉ step đầu có giờ
                    ReservedEndTime = i == 0
                        ? TimeOnly.FromTimeSpan(serviceHour.ToTimeSpan().Add(TimeSpan.FromMinutes(step.Duration)))
                        : defaultTime, // Các step sau không có giờ
                    Date = currentDate // Gán ngày đã tính toán
                };

                schedules.Add(schedule);

                // Nếu không phải step cuối cùng, cộng thêm DayToNextStep của step hiện tại vào ngày hiện tại
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
    }
}
