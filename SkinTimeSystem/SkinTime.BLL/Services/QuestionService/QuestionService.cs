using Microsoft.EntityFrameworkCore;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.QuestionService
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public QuestionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<Question>> GetAllQuestion()
        {
            return _unitOfWork.Repository<Question>().GetAllAsync(q => q.QuestionOptions);
        }

        public async Task<(Dictionary<SkinType, double> SkinTypes, List<Service> Services)> GetServiceRecommments(Guid userId, List<Guid> listResult)
        {
            if (!listResult.Any())
            {
                return (new Dictionary<SkinType, double>(), new List<Service>());
            }

            var userChoices = listResult.Select(qid => new UserChoice
            {
                UserID = userId,
                QuestionOptionID = qid,
                CreatedTime = DateTime.Now
            }).ToList();
            await _unitOfWork.Repository<UserChoice>().AddRangeAsync(userChoices);
            await _unitOfWork.Complete();

            var allSkinTypes = await _unitOfWork.Repository<SkinType>().ListAsync();

            var questionOptions = await _unitOfWork.Repository<QuestionOption>()
                .ListAsync(qo => listResult.Contains(qo.Id), null, qo => qo.Include(q => q.SkinType));

            if (!questionOptions.Any())
            {
                return (allSkinTypes.ToDictionary(st => st, st => 0.0), new List<Service>());
            }

            var skinTypeCounts = questionOptions
                .GroupBy(qo => qo.SkinType)
                .ToDictionary(g => g.Key, g => g.Count());

            int totalSelections = skinTypeCounts.Values.Sum();

            var skinTypePercentages = allSkinTypes.ToDictionary(
                st => st,
                st => skinTypeCounts.ContainsKey(st) ? (double)skinTypeCounts[st] / totalSelections * 100 : 0.0
            );

            var highestSkinType = skinTypePercentages.OrderByDescending(st => st.Value).FirstOrDefault().Key;

            if (highestSkinType == null) return (skinTypePercentages, new List<Service>());

            var recommendedServices = await _unitOfWork.Repository<ServiceRecommendation>()
               .ListAsync(sr => sr.SkinTypeID == highestSkinType.Id, null);

            var serviceIds = recommendedServices.Select(sr => sr.ServiceID).Distinct().ToList();

            var services = await _unitOfWork.Repository<Service>()
                .ListAsync(s => serviceIds.Contains(s.Id), null);

            return (skinTypePercentages, services.ToList());
        }

    }
}
