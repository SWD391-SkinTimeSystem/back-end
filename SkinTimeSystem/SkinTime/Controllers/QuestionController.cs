using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Services.QuestionService;
using SkinTime.DAL.Entities;
using SkinTime.Models;

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
        public async Task<ActionResult<ApiResponse<ICollection<QuestionModel>>>> GetAllQuestion()
        {
            return await HandleServiceCall<ICollection<QuestionModel>>(async () =>
            {
                return await _service.GetAllQuestion();
            });
           
        }

        [HttpPost("recommendations")]
        public async Task<IActionResult> GetServiceRecommments([FromBody] AnswerModel answer)
        {
            return await HandleApiCallAsync(async () =>
            {
                var serviceRecomment = await _service.GetServiceRecommments(answer.UserId, answer.ResultIds);
                var serviceRecommendation = _mapper.Map<AnalysisModel>(serviceRecomment);
                return serviceRecommendation;
            });

        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] ICollection<QuestionCreationModel> quiz)
        {
            return await HandleServiceCall(async () =>
            {
                return await _service.CreateOrUpdateQuestions(_mapper.Map<ICollection<Question>>(quiz));
            });
        }

    }
}
