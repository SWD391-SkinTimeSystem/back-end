using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.DTOs;
using SkinTime.DTOs.Therapist;
using SkinTime.Helpers;

namespace SkinTime.Controllers
{
    [Route("api/therapist")]
    [ApiController]
    public class TherapistController : BaseController
    {
        private readonly ITherapistService _service;

        public TherapistController(IMapper mapper, IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities, ITherapistService service)
            : base(mapper, emailUtilities, tokenUtilities)
        {
            _service = service;
        }

        /// <summary>
        ///     Get the list of all therapist (including removed ones).
        /// </summary>
        /// <returns>The 200 Ok action result with data as list of therapist.</returns>
        [HttpGet]
        [ProducesResponseType<ApiResponse<ICollection<TherapistViewModel>>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<ICollection<TherapistViewModel>>>> GetTherapistList()
        {
            return await HandleServiceCall<ICollection<TherapistViewModel>>(async () =>
            {
                return await _service.GetAllTherapist();
            });
        }

        /// <summary>
        ///     Find first available therapist based on the given date, time and the service duration.
        /// </summary>
        /// <param name="date">the date to check (ex: "2025/11/25")</param>
        /// <param name="time">the time to check (ex: "14:15:00")</param>
        /// <param name="duration">the duration of the service</param>
        /// <returns>A list of therapist that match the given criteria</returns>
        [HttpGet("available")]
        [ProducesResponseType<ApiResponse<TherapistViewModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvailableTherapistForDay(DateOnly date, TimeOnly time, int duration)
        {
            return await HandleServiceCall<TherapistViewModel>(async () =>
            {
                return await _service.GetFirstAvailableTherapist(date, time, duration);
            });

        }

        /// <summary>
        ///     Get a therapist information with given id.
        /// </summary>
        /// <param name="id">The therapist id</param>
        /// <returns>The 200 Ok action result with therapist information that match the provided id, else a 404 Not Found result.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType<ApiResponse<ICollection<TherapistViewModel>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TherapistViewModel>> GetTherapistInformation(Guid id)
        {
            return await HandleServiceCall<TherapistViewModel>(async () =>
            {
                return await _service.GetTherapistWithId(id);
            });
        }
    }
}
