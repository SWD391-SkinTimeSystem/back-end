using SkinTime.BLL.Commons;
using SkinTime.BLL.Commons.DTOs.StatisticDTOs;
using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.StatisticService
{
    public interface IStatisticService
    {
        Task<ServiceResult<OverviewStatisticsDTO>> GetOverviewStatistics(DateOnly? from, DateOnly? to);

        Task<ServiceResult<ICollection<RevenueDTO>>> GetDailyRevenueStatistics(DateOnly? from, DateOnly? to);

        Task<ServiceResult<ICollection<Service>>> GetMostPopularService(int limit = 5, bool includeDeleted = false);

        Task<ServiceResult<Dictionary<string, int>>> GetBookingStatisticByStatus(DateOnly? from, DateOnly? to);

        Task<ServiceResult<Dictionary<string, int>>> GetEventStatisticByStatus(DateOnly? from, DateOnly? to);
    }
}
