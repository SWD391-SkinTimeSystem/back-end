using BusinessObject.Entities;
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
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> GetScheduleById(Guid scheduleId)
        {
            var schedule =  _context.Schedules.SingleOrDefault(x => x.Id == scheduleId);
            if(schedule == null)
            {
                return false;
            }
            return true;
        }

        public async Task<Schedule> ReSchedule(Guid idSchedule, DateOnly date, TimeOnly time)
        {
            var schchedule = _context.Schedules.SingleOrDefault(x => x.Id == idSchedule);
            var serviceDetail = _context.ServiceDetails.SingleOrDefault(s => s.Id == schchedule.ServiceDetailId);
            schchedule.Date = date;
            schchedule.ReservedEndTime = time.AddMinutes(serviceDetail.Duration);
            schchedule.ReservedStartTime = time;
            schchedule.ReservedEndTime = time.AddMinutes(30);
            schchedule.LastUpdate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return schchedule;
        }

    }
}
