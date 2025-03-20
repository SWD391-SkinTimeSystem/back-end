using BusinessObject.Entities;
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
    }
}
