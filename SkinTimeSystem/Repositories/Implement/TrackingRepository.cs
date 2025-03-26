using BusinessObject.Entities;
using BusinessObject.Schedule;
using Repositories.Data;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    public class TrackingRepository :  GenericRepository<Tracking>, ITrackingRepository
    {
        public TrackingRepository(ApplicationDbContext context) : base(context) { }

        

        public async Task<Tracking> CreateTracking(Tracking tracking)
        {
            var schedule =  _context.Schedules.SingleOrDefault(x => x.Id == tracking.ScheduleId);
            schedule.Status = ScheduleStatus.Doing;
            tracking.TherapistId = schedule.BookingNavigation.TherapistId;
            tracking.CheckinTime = DateTime.Now;
            tracking.Id = Guid.NewGuid();
            _context.Trackings.Add(tracking);
            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();
            return tracking;
        }

        public async Task<bool> NoteTracking(Guid trackingId, string note)
        {
            var tracking = _context.Trackings.SingleOrDefault(x => x.Id == trackingId);
            tracking.Note = note;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckoutTracking(Guid trackingId)
        {
            var tracking = _context.Trackings.SingleOrDefault(x => x.Id == trackingId);
            var schedule = _context.Schedules.SingleOrDefault(x => x.Id == tracking.ScheduleId);
            tracking.CheckoutTime = DateTime.Now;
            schedule.Status = ScheduleStatus.Completed;
            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();
            return true;
        }

        

        public async Task<Tracking> GetTrackingWithSchedulId(Guid scheduleId)
        {
            var tracking = _context.Trackings.SingleOrDefault(x => x.ScheduleId == scheduleId);
            return tracking;
        }

        



    }
}
