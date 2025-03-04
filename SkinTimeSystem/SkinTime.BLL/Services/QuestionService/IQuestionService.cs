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
        Task<(Dictionary<SkinType, double> SkinTypes, List<Service>? Services)> GetServiceRecommments( List<Guid> listResult);
    }
}
