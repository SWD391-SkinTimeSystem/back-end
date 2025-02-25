using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Commons.DTOs.StatisticDTOs;
using SkinTime.BLL.Services.StatisticService;
using SkinTime.Models;

namespace SkinTime.Controllers
{
    /// <summary>
    ///     Endpoints used for dashboard statistics and other information display purposes.
    /// </summary>
    [Route("api/statistic")]
    [ApiController]
    public class StatisticController : BaseController
    {
        private readonly IStatisticService _service;

        public StatisticController(IMapper mapper, IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities, IStatisticService service)
            : base(mapper, emailUtilities, tokenUtilities)
        {
            _service = service;
        }

        /// <summary>
        ///     Get a list of overview statistic from a date range.
        ///     By default this will get the current date statistic
        /// </summary>
        /// <param name="from">Starting date</param>
        /// <param name="to">Ending date</param>
        /// <returns></returns>
        [HttpGet("overview")]
        [ProducesResponseType<ApiResponse<OverviewStatisticsDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOverviewStatistic(DateOnly? from, DateOnly? to)
        {
            return await HandleServiceCall(async () =>
            {
                return await _service.GetOverviewStatistics(from, to);
            });
            
        }

        /// <summary>
        ///     Get a list of revenue and their breakdown.
        ///     By default this will get the current date statistic
        /// </summary>
        /// <param name="from">starting date</param>
        /// <param name="to">ending date</param>
        /// <returns></returns>
        [HttpGet("revenue")]
        [ProducesResponseType<ApiResponse<ICollection<RevenueDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDailyRevenueBreakdown(DateOnly? from, DateOnly? to)
        {
            return await HandleServiceCall(async () =>
            {
                return await _service.GetDailyRevenueStatistics(from, to);
            });
        }

        /// <summary>
        ///     Get number of booking based on the booking status.
        ///     By default this will get the current date statistic
        /// </summary>
        /// <param name="from">The starting date</param>
        /// <param name="to">The ending date</param>
        /// <returns></returns>
        [HttpGet("booking/status")]
        [ProducesResponseType<ApiResponse<IDictionary<string, int>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBookingByStatus(DateOnly? from, DateOnly? to)
        {
            return await HandleServiceCall(async () =>
            {
                return await _service.GetBookingStatisticByStatus(from, to);
            });
        }

        /// <summary>
        ///     Get the upcoming bookings (booking with status of NotStarted)
        /// </summary>
        /// <param name="limit">The maximum amount of booking information to return</param>
        /// <returns></returns>
        [HttpGet("booking/upcoming")]
        [ProducesResponseType<ApiResponse<ICollection<BookingViewModel>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUpcomingBookingIngo(int limit = 5)
        {
            return await HandleServiceCall<ICollection<BookingViewModel>>(async () =>
            {
                return await _service.GetUpcomingBookings(limit);
            });
        }

        /// <summary>
        ///     Get a list of most popular services of all time.
        /// </summary>
        /// <param name="limit">limit to the number of services</param>
        /// <returns></returns>
        [HttpGet("popular/service")]
        [ProducesResponseType<ApiResponse<ICollection<PopularServicesViewModel>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPopularServices(int limit = 5)
        {
            return await HandleServiceCall<ICollection<PopularServicesViewModel>>(async () =>
            {
                return await _service.GetMostPopularService(limit);
            });
        }

        /// <summary>
        ///     Get the number of event grouped by status in a range of time.
        ///     By default this will get the current date statistic.
        /// </summary>
        /// <param name="from">The starting date</param>
        /// <param name="to">The ending date</param>
        /// <returns></returns>
        [HttpGet("event/status")]
        [ProducesResponseType<ApiResponse<IDictionary<string, int>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventStatisticByStatus(DateOnly? from, DateOnly? to)
        {
            return await HandleServiceCall(async () =>
            {
                return await _service.GetEventStatisticByStatus(from, to);
            });
        }

        /// <summary>
        ///     Get the event related statistics in a date range.
        ///     By default this will only get the current date statistic.
        /// </summary>
        /// <param name="from">The starting date</param>
        /// <param name="to">The ending date</param>
        /// <returns></returns>
        [HttpGet("event")]
        [ProducesResponseType<ApiResponse<EventStatisticDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventStatistic(DateOnly? from, DateOnly? to)
        {
            return await HandleServiceCall(async () =>
            {
                return await _service.GetDailyEventStatistics(from, to);
            });
        }
    }
}
