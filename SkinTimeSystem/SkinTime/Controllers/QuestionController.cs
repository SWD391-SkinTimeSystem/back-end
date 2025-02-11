using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkinTime.BLL.Services.QuestionService;
using SkinTime.DAL.Entities;
using SkinTime.Models;

namespace SkinTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class questionController : BaseController
    {
        private readonly IQuestionService _service;
        private readonly IMapper _mapper;

        public questionController(IQuestionService service,
            IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllQuestion()
        {
            return await HandleApiCallAsync(async () =>
            {
                var allQuestion = await _service.GetAllQuestion();
                var allQuestionDTO = _mapper.Map<List<QuestionModel>>(allQuestion);
                return allQuestionDTO;
            });
           
        }

        [HttpPost("recommendations")]
        public async Task<IActionResult> GetServiceRecommments([FromBody] AnswerModel answer)
        {
            var serviceRecomment = await _service.GetServiceRecommments(answer.UserId, answer.ResultIds);
            var serviceRecommendationDto = _mapper.Map<AnalysisModel>(serviceRecomment);
            return Ok(serviceRecommendationDto);
        }

    }
}
