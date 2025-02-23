using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Commons;
using SkinTime.BLL.Services.ScheduleService;
using SkinTime.DAL.Entities;
using SkinTime.Helpers;
using SkinTime.Models;

namespace SkinTime.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController : BaseController
    {
        IScheduleService _service;

        public ScheduleController(IMapper mapper,IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities, IScheduleService service)
        : base(mapper, emailUtilities, tokenUtilities)
        {
            this._service = service;
        }

        /// <summary>
        ///     Generate a new schedule based on the booking id and the previous schedule. This is used for view purposes only.
        /// </summary>
        /// <param name="id">The booking id as string</param>
        /// <remarks>Only staffs can use this endpoint to create schedules.</remarks>
        /// <returns>The precalculated new schedule.</returns>
        [Authorize(Roles = "Staff")]
        [HttpGet("{id}/create")]
        public async Task<ActionResult<ScheduleViewModel>> GetPreCalculatedSchedule(string id)
        {
            return await HandleServiceCall<Schedule, ScheduleViewModel>(async () =>
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
        public async Task<ActionResult<ScheduleViewModel>> GetScheduleWithId(Guid id)
        {
            return await HandleServiceCall<Schedule, ScheduleViewModel>( async () =>
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
        public async Task<ActionResult<ICollection<ScheduleViewModel>>> GetPersonalSchedule()
        {
            // Get the user id from jwt token.
            string jwtToken = Request.Headers.Authorization.Single()!;
            Guid userId = Guid.Parse(_tokenUtils.GetDataDictionaryFromJwt(jwtToken.Split()[1])["id"]);

            return await HandleServiceCall<ICollection<Schedule>, ICollection<ScheduleViewModel>>(async () =>
            {
                return await _service.GetUserSchedules(userId);
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
        public async Task<ActionResult<ICollection<ScheduleViewModel>>> GetPersonalSchedule(int? year, int? week)
        {
            // Get the user id from jwt token.
            string jwt = Request.Headers.Authorization.Single()!;
            Guid userId = Guid.Parse(_tokenUtils.GetDataDictionaryFromJwt(jwt.Split()[1])["id"]);
            
            // Get the start and end date from the input week.
            DateOnly startOfWeek, endOfWeek;
            if (week != null) 
            {
                DateOnly yearStart = new DateOnly(year != null ? (int) year : DateTime.UtcNow.Year, 1, 1);

                startOfWeek = yearStart.AddDays(7 * ((int) week - 1) - (int) DateTime.UtcNow.DayOfWeek + 1);
                endOfWeek = yearStart.AddDays(7);
            }
            else {
                startOfWeek = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1 * (int)DateTime.UtcNow.DayOfWeek);
                endOfWeek = startOfWeek.AddDays(7);
            }

            return await HandleServiceCall<ICollection<ScheduleViewModel>>(async () =>
            {
                var result = await _service.GetUserSchedules(userId);

                var collection = result.Data!.Where(x => startOfWeek <= x.Date && x.Date <= endOfWeek).ToList();

                return ServiceResult.Success(collection);
            });
        }
    }
}
