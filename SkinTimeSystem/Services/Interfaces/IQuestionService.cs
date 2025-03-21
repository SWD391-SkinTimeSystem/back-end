using BusinessObject.Entities;
using Services.Commons;
using Services.Commons.DTOs.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IQuestionService
    {
        Task<ICollection<Question>> GetAllQuestion();

        Task<ServiceResult> CreateQuestion(Question questions);

        Task<ServiceResult> UpdateQuestion(Question questions);

        Task<ServiceResult> UpdateAllQuestion(ICollection<QuestionCreationDTO> questions);

        Task<ServiceResult> DeleteQuestion(Question questions);

        Task<ServiceResult> CreateQuestions(ICollection<Question> questions);

        Task<(Dictionary<SkinType, double> SkinTypes, List<Service>? Services)> GetServiceRecommments(List<Guid> listResult);
    }
}
