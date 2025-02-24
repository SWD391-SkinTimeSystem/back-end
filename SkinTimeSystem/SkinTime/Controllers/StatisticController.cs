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
        /// </summary>
        /// <param name="from">The starting date</param>
        /// <param name="to">The ending date</param>
        /// <returns></returns>
        [HttpGet("booking/status")]
        [ProducesResponseType<ApiResponse<IDictionary<string, int>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(DateOnly? from, DateOnly? to)
        {
            return await HandleServiceCall(async () =>
            {
                return await _service.GetBookingStatisticByStatus(from, to);
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
        public async Task<IActionResult> Get(int limit = 5)
        {
            return await HandleServiceCall<ICollection<PopularServicesViewModel>>(async () =>
            {
                return await _service.GetMostPopularService(limit);
            });
        }
    }
}
