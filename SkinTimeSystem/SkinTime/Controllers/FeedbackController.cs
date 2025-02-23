using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Commons;
using SkinTime.BLL.Services.FeedbackService;
using SkinTime.DAL.Entities;
using SkinTime.Models;

namespace SkinTime.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService _service;

        public FeedbackController(IMapper mapper, IEmailUtilities emailUtils, ITokenUtilities tokenUtils, IFeedbackService feedbackService)
        : base(mapper, emailUtils, tokenUtils)
        {
            _service = feedbackService;
        }

        /// <summary>
        ///     Create a new feedback for a finished (or canceled) service booking
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns>A <see cref="ApiResponse"/> if success, an error response if failed.</returns>
        [Authorize(Roles = "Customer")]
        [HttpPost("booking/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateFeedback([FromBody] FeedbackCreationModel feedback)
        {
            return await HandleServiceCall<ApiResponse>(async () =>
            {
                var result = await _service.CreateNewFeedback(_mapper.Map<Feedback>(feedback));
                if (result.IsFailed)
                {
                    return result;
                }
                return ServiceResult.Success(new ApiResponse(true, "Successfully added the feedback"));
            });
        }

        /// <summary>
        ///     Get all the feedback exist in the system. Only available for manager and admin.
        /// </summary>
        /// <returns>An <see cref="ApiResponse{T}"/> with a list of all booking feedback made by users.</returns>
        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        [ProducesResponseType<ApiResponse<ICollection<BookingFeedbackViewModel>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<ICollection<BookingFeedbackViewModel>>>> GetAllFeedBack()
        {
            return await HandleServiceCall<ICollection<BookingFeedbackViewModel>>(async () =>
            {
                return await _service.GetAllFeedback();
            });
        }

        /// <summary>
        ///     Get all the feedback belong to a customer.
        /// </summary>
        /// <param name="id">customer's user id as a string</param>
        /// <returns>An <see cref="ApiResponse{T}"/> with a list of all booking feedback made by user with provided id.</returns>
        [Authorize(Roles = "Customer")]
        [HttpGet("customer/{id}")]
        [ProducesResponseType<ApiResponse<ICollection<BookingFeedbackViewModel>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ICollection<BookingFeedbackViewModel>>> GetAllCustomerFeedback(string id)
        {
            // Get user id from jwt.
            Dictionary<string, string> data = _tokenUtils.GetDataDictionaryFromJwt(Request.Headers.Authorization.Single()!.Split()[1]);

            // if the customer and the searching id does not match, we do not allow operation.
            return await HandleServiceCall<ICollection<BookingFeedbackViewModel>>(async () =>
            {
                if (data["id"] != id)
                {
                    return ServiceResult<ICollection<BookingFeedbackViewModel>>
                    .Failed(ServiceError.Unauthorized("the requested id does not match with user id!"));
                }

                return await _service.GetAllUserFeedback(id);
            });
        }

        /// <summary>
        ///     Get a booking feedback using booking id.
        /// </summary>
        /// <param name="booking">the booking id</param>
        /// <returns>The <see cref="ApiResponse"/> with booking feedback if found, else return 4xx response.</returns>
        [Authorize(Roles = "Customer,Manager,Admin")]
        [HttpGet("booking/{booking}")]
        [ProducesResponseType<ApiResponse<ICollection<BookingFeedbackViewModel>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse>> GetBookingFeedbackDetail(string booking)
        {
            // Get user id from jwt.
            Dictionary<string, string> tokenData = _tokenUtils.GetDataDictionaryFromJwt(Request.Headers.Authorization.Single()!.Split()[1]);
            string bookingId = tokenData["id"];

            return await HandleServiceCall<ICollection<BookingFeedbackViewModel>>(async () =>
            {
                return await _service.GetBookingFeedback(bookingId);
            });
        }

        /// <summary>
        ///     Only return therapist related feedback data of a therapist.
        /// </summary>
        /// <param name="id">The therapist id</param>
        /// <returns></returns>
        [HttpGet("therapist/{id}")]
        [ProducesResponseType<ApiResponse<ICollection<TherapistFeedbackViewModel>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetTherapistFeedback(string id)
        {
            return await HandleServiceCall<ICollection<TherapistFeedbackViewModel>>(async() =>
            {
                return await _service.GetAllTherapistFeedback(id);
            });
        }

        /// <summary>
        ///     Only return service related feedback data of a service.
        /// </summary>
        /// <param name="id">The therapist id</param>
        /// <returns></returns>
        [ProducesResponseType<ApiResponse<ICollection<ServiceFeedbackViewModel>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [HttpGet("service/{id}")]
        public async Task<ActionResult<ApiResponse>> GetServiceFeedback(string id)
        {
            return await HandleServiceCall<ICollection<ServiceFeedbackViewModel>>(async () =>
            {
                return await _service.GetAllServiceFeedback(id);
            });
        }
    }
}
