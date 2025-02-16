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
        Task CreateOrUpdateQuestions(ICollection<Question> questions);
        Task<List<Question>> GetAllQuestion();
        Task<(Dictionary<SkinType, double> SkinTypes, List<Service>? Services)> GetServiceRecommments(Guid userId, List<Guid> listResult);
    }
}
