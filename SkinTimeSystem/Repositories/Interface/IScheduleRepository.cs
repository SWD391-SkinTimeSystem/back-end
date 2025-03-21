using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        Task<bool> GetScheduleById(Guid scheduleId);
        Task<Schedule> ReSchedule(Guid iDSchedule,DateOnly date, TimeOnly time);
        Task<bool> CheckFirstStep(Guid idSchedule);
    }
}
