using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.QuestionService
{
    public interface IQuestionService
    {
        Task<ICollection<Question>> GetAllQuestion();

        Task<ServiceResult> CreateQuestion(Question questions);

        Task<ServiceResult> UpdateQuestion(Question questions);

        Task<ServiceResult> UpdateAllQuestion(ICollection<Question> questions);

        Task<ServiceResult> DeleteQuestion(Question questions);

        Task<ServiceResult> CreateQuestions(ICollection<Question> questions);

        Task<(Dictionary<SkinType, double> SkinTypes, List<Service>? Services)> GetServiceRecommments( List<Guid> listResult);
    }
}
