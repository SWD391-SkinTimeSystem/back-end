using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext context) : base(context) { }
        public async Task<Dictionary<SkinType, double>> GetSkinTypePercentagesAsync(List<Guid> listResult)
        {
            var allSkinTypes = await _context.Set<SkinType>().ToListAsync();

            var questionOptions = await _context.Set<QuestionOption>()
                .Include(qo => qo.SkinTypes)
                .Where(qo => listResult.Contains(qo.Id))
                .ToListAsync();

            if (!questionOptions.Any())
            {
                return allSkinTypes.ToDictionary(st => st, st => 0.0);
            }

            var skinTypeCounts = questionOptions
                .SelectMany(qo => qo.SkinTypes)
                .GroupBy(st => st)
                .ToDictionary(g => g.Key, g => g.Count());

            int totalSelections = skinTypeCounts.Values.Sum();

            var skinTypePercentages = allSkinTypes.ToDictionary(
                st => st,
                st => skinTypeCounts.ContainsKey(st)
                    ? Math.Round((double)skinTypeCounts[st] / totalSelections * 100, 1)
                    : 0.0
            );

            return skinTypePercentages;
        }
        public async Task<ICollection<Question>> GetAllQuestionsAsync()
        {
            return await _context.Set<Question>()
                .Include(q => q.QuestionOptionsNavigation)
                .ThenInclude(qo => qo.SkinTypes)
                .ToListAsync();
        }
    }
}
