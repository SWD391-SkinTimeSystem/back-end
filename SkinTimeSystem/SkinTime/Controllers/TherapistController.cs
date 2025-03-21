using API.Model;
using AutoMapper;
using BusinessObject.Enum;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Services.Commons;
using Services.Commons.DTOs.Therapist;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
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
        ///     Get the list of all therapist (filter by status).
        /// </summary>
        /// <returns>The 200 Ok action result with data as list of therapist.</returns>
        [HttpGet]
        [ProducesResponseType<ApiResponse<PaginationResult<TherapistDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTherapistList(int page = 1, int page_size = 20, TherapistStatus status = TherapistStatus.Available)
        {
            PaginationResult result = await _service.GetAllTherapistWithStatus(page, page_size, status);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "success",
                Data = result,
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
        [ProducesResponseType<ApiResponse<TherapistDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvailableTherapistForDay(DateOnly date, TimeOnly time, int duration)
        {
            ServiceResult result = await _service.GetFirstAvailableTherapist(date, time, duration);

            return HandleServiceCall(result);

        }

        [HttpGet("available-list")]
        [ProducesResponseType<ApiResponse<ICollection<TherapistDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvailableTherapistsForDay(DateOnly date, TimeOnly time, int duration)
        {
            ICollection<TherapistDTO> result = await _service.GetAvailableTherapist(date, time, duration);

            return Ok( new ApiResponse
            {
                Success = true,
                Message= "success",
                Data= result,
            });

        }

        /// <summary>
        ///     Get a therapist information with given id.
        /// </summary>
        /// <param name="id">The therapist id</param>
        /// <returns>The 200 Ok action result with therapist information that match the provided id, else a 404 Not Found result.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType<ApiResponse<TherapistDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTherapistInformation(Guid id)
        {
            ServiceResult result = await _service.GetTherapistWithId(id);

            return HandleServiceCall(result);
        }
    }
}
