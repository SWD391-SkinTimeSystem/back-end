using BusinessObject.Entities;
using Services.Commons;
using Services.Commons.DTOs.StatisticDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStatisticService
    {
        Task<ServiceResult<OverviewStatisticsDTO>> GetOverviewStatistics(DateOnly? from, DateOnly? to);

        Task<ServiceResult<ICollection<RevenueDTO>>> GetDailyRevenueStatistics(DateOnly? from, DateOnly? to);

        Task<ServiceResult<SingleRevenueDTO>> GetDailyEventRevenueStatistics(DateOnly? from, DateOnly? to);

        Task<ServiceResult<SingleRevenueDTO>> GetDailyBookingRevenueStatistics(DateOnly? from, DateOnly? to);

        Task<ServiceResult<EventStatisticDTO>> GetDailyEventStatistics(DateOnly? from, DateOnly? to);

        Task<ServiceResult<ICollection<Service>>> GetMostPopularService(int limit = 5, bool includeDeleted = false);

        Task<ServiceResult<Dictionary<string, int>>> GetBookingStatisticByStatus(DateOnly? from, DateOnly? to);

        Task<ServiceResult<Dictionary<string, int>>> GetEventStatisticByStatus(DateOnly? from, DateOnly? to);

        Task<ServiceResult<ICollection<Booking>>> GetUpcomingBookings(int limit = 5);
    }
}
