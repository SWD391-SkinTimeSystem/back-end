using AutoMapper;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Commons;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.DTOs;
using SkinTime.DTOs.Analysis;
using SkinTime.DTOs.Question;

namespace SkinTime.Controllers
{
    [Route("api/question")]
    [ApiController]
    public class QuestionController : BaseController
    {
        private readonly IQuestionService _service;
        public QuestionController(IQuestionService service,
            IMapper mapper, IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities)
            : base(mapper, emailUtilities, tokenUtilities)
        {
            _service = service;
        }

        /// <summary>
        ///     Get the list of questions (and corresponding options)
        /// </summary>
        /// <returns>The list of questions</returns>
        [HttpGet]
        [ProducesResponseType<ApiResponse<ICollection<QuestionModel>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllQuestion()
        {
            return await HandleServiceCall<ICollection<QuestionModel>>(async () =>
            {
                return ServiceResult.Success(await _service.GetAllQuestion());
            });
        }

        /// <summary>
        ///     Update quiz questions and choices.
        /// </summary>
        /// <param name="questions">list of question and choices</param>
        /// <returns>200 status response if ok, else bad request.</returns>
        //[Authorize(Roles = "Therapist")]
        [HttpPost]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateQuestionList([FromBody] ICollection<QuestionCreationModel> questions)
        {
            Func<Task<ServiceResult>> function = async () =>
            {
                return await _service.UpdateAllQuestion(_mapper.Map<ICollection<Question>>(questions));
            };

            return await HandleServiceCall(function);
        }

        /// <summary>
        ///     Get the user sin types and recommended services based on the user selected quiz choices. 
        /// </summary>
        /// <param name="answer">The list of choices user has selected.</param>
        /// <returns>skin types in percentages and list of recommended services.</returns>
        [HttpPost("recommendations")]
        [ProducesResponseType<ApiResponse<AnalysisModel>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServiceRecommments([FromBody] AnswerModel answer)
        {
            return await HandleServiceCall<AnalysisModel>(async () =>
            {
                return ServiceResult.Success(await _service.GetServiceRecommments(answer.ResultIds));
            });
        }
    }
}
