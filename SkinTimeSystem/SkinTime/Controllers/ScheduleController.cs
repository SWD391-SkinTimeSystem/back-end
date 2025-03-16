using API.Model;
using AutoMapper;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Commons;
using Services.Commons.DTOs.Schedule;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;

namespace SkinTime.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController : BaseController
    {
        IScheduleService _service;

        public ScheduleController(IMapper mapper, IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities, IScheduleService service)
        : base(mapper, emailUtilities, tokenUtilities)
        {
            this._service = service;
        }

        /// <summary>
        ///     Get the availability of a therapist for the current date (and the next 6 days)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("therapist/{id}/availability")]
        [ProducesResponseType<ApiResponse<TherapistAvailabilityDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTherapistAvailabilitySchedule(string id)
        {
            return await HandleServiceCall(async () =>
            {
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
                ServiceResult<ICollection<Schedule>> result = await _service.GetTherapistSchedule(id, currentDate, currentDate.AddDays(6));

                if (result.IsFailed)
                {
                    return result;
                }

                // Manually mapping to result return type.
                TherapistAvailabilityDTO viewModel = new TherapistAvailabilityDTO
                {
                    TherapistId = Guid.Parse(id),
                    Availability = new Dictionary<DateOnly, IDictionary<TimeOnly, bool>>(),
                };

                TimeOnly startOfDay = TimeOnly.Parse("9:00:00");
                TimeOnly endOfDay = TimeOnly.Parse("17:00:00");

                for (DateOnly x = currentDate; x <= currentDate.AddDays(6); x = x.AddDays(1))
                {
                    viewModel.Availability[x] = new Dictionary<TimeOnly, bool>();

                    for (TimeOnly y = startOfDay; y <= endOfDay; y = y.AddMinutes(30))
                    {
                        viewModel.Availability[x][y] = !result.Data!
                        .Any(s => s.Date == x && (s.ReservedStartTime <= y && y < s.ReservedEndTime));
                    }
                }

                return ServiceResult.Success(viewModel);
            });
        }

        [HttpGet("availability")]
        public async Task<IActionResult> GetAvailabilityForDate([FromQuery] DateOnly date)
        {
            return await HandleServiceCall(async () =>
            {
                return await _service.GetScheduleForDate(date);
            });
        }

        /// <summary>
        ///     Generate a new schedule based on the booking id and the previous schedule. This is used for view purposes only.
        /// </summary>
        /// <param name="id">The booking id as string</param>
        /// <remarks>Only staffs can use this endpoint to create schedules.</remarks>
        /// <returns>The precalculated new schedule.</returns>
        [Authorize(Roles = "Staff")]
        [HttpGet("{id}/create")]
        public async Task<ActionResult<ScheduleDTO>> GetPreCalculatedSchedule(string id)
        {
            return await HandleServiceCall<Schedule, ScheduleDTO>(async () =>
            {
                return await _service.GenerateScheduleForBooking(id);
            });
        }

        /// <summary>
        ///     <para>
        ///         Return the schedule detail with the provided id.
        ///     </para>
        /// </summary>
        /// <remarks>ANYONE can use this endpoint to get ANY schedule detail</remarks>
        /// <param name="id"></param>
        /// <returns>The user scheduled resevation (not available slot)</returns>
        [HttpGet("{id}")]
        [ProducesResponseType<ApiResponse<ScheduleDTO>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<ScheduleDTO>> GetScheduleWithId(Guid id)
        {
            return await HandleServiceCall<Schedule, ScheduleDTO>(async () =>
            {
                return await _service.GetSchedule(id);
            });
        }

        /// <summary>
        ///     <para>
        ///         Return the current (logged in) user scheduled reservation.
        ///     </para>
        /// </summary>
        /// <remarks>Only users with customer or therapist role may use this method.</remarks>
        /// <returns>The user scheduled resevation (not available slot)</returns>
        [Authorize(Roles = "Customer,Therapist")]
        [HttpGet]
        public async Task<ActionResult<ICollection<ScheduleDTO>>> GetPersonalSchedule()
        {
            // Get the user id from jwt token.
            string jwtToken = Request.Headers.Authorization.Single()!;
            Guid userId = Guid.Parse(_tokenUtils.GetDataDictionaryFromJwt(jwtToken.Split()[1])["id"]);

            return await HandleServiceCall<ICollection<Schedule>, ICollection<ScheduleDTO>>(async () =>
            {
                var schedules = await _service.GetUserSchedules(userId);
                return schedules;
            });
        }

        /// <summary>
        ///     <para>
        ///         Return the current (logged in) user scheduled reservation.
        ///     </para>
        /// </summary>
        /// <remarks>Customer and Therapist user role only.</remarks>
        /// <param name="week">The year to get the schedule, left null for the default as current year</param>
        /// <param name="year">The week of the year, left null for the default as current week</param>
        /// <returns>The user scheduled resevation (not available slot)</returns>
        [Authorize(Roles = "Customer,Therapist")]
        [HttpGet("week")]
        public async Task<ActionResult<ICollection<ScheduleDTO>>> GetPersonalSchedule(int? year, int? week)
        {
            // Get the user id from jwt token.
            string jwt = Request.Headers.Authorization.Single()!;
            Guid userId = Guid.Parse(_tokenUtils.GetDataDictionaryFromJwt(jwt.Split()[1])["id"]);

            // Get the start and end date from the input week.
            DateOnly startOfWeek, endOfWeek;
            if (week != null)
            {
                DateOnly yearStart = new DateOnly(year != null ? (int)year : DateTime.UtcNow.Year, 1, 1);

                startOfWeek = yearStart.AddDays(7 * ((int)week - 1) - (int)DateTime.UtcNow.DayOfWeek + 1);
                endOfWeek = yearStart.AddDays(7);
            }
            else
            {
                startOfWeek = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1 * (int)DateTime.UtcNow.DayOfWeek);
                endOfWeek = startOfWeek.AddDays(7);
            }

            return await HandleServiceCall<ICollection<ScheduleDTO>>(async () =>
            {
                var result = await _service.GetUserSchedules(userId);

                var collection = result.Data!.Where(x => startOfWeek <= x.Date && x.Date <= endOfWeek).ToList();

                return ServiceResult.Success(collection);
            });
        }
    }
}
