using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<Dictionary<SkinType, double>> GetSkinTypePercentagesAsync(List<Guid> listResult);
        Task<ICollection<Question>> GetAllQuestionsAsync();
    }
}
