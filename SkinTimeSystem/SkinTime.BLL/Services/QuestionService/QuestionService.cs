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

        public Task<List<Question>> GetAllQuestion() =>_unitOfWork.Repository<Question>().GetAllAsync(q => q.QuestionOptions);
        

        public async Task<(Dictionary<SkinType, double> SkinTypes, List<Service> Services)> GetServiceRecommments(Guid userId, List<Guid> listResult)
        {
            var userChoices = listResult.Select(qid => new UserChoice
            {
                UserID = userId,
                QuestionOptionID = qid,
                CreatedTime = DateTime.Now
            }).ToList();
            await _unitOfWork.Repository<UserChoice>().AddRangeAsync(userChoices);
            await _unitOfWork.Complete();

            var allSkinTypes = await _unitOfWork.Repository<SkinType>().ListAsync();// lấy tất cả các loại skin type

            var questionOptions = await _unitOfWork.Repository<QuestionOption>()
                .ListAsync(qo => listResult.Contains(qo.Id), null, qo => qo.Include(q => q.SkinType));// Lấy thông tin của các lựa chọn của người dùng

            if (!questionOptions.Any())
            {
                return (allSkinTypes.ToDictionary(st => st, st => 0.0), new List<Service>());
            }

            var skinTypeCounts = questionOptions
                .GroupBy(qo => qo.SkinType)
                .ToDictionary(g => g.Key, g => g.Count()); // 

            int totalSelections = skinTypeCounts.Values.Sum();

            var skinTypePercentages = allSkinTypes.ToDictionary(
              st => st,
              st => skinTypeCounts.ContainsKey(st)
              ? Math.Round((double)skinTypeCounts[st] / totalSelections * 100, 1)
              : 0.0);


            var maxPercentage = skinTypePercentages.Max(st => st.Value); // Tìm % cao nhất

            var highestSkinTypes = skinTypePercentages
                .Where(st => st.Value == maxPercentage) // Lấy tất cả loại da có cùng % cao nhất
                .Select(st => st.Key)
                .ToList();

            if (!highestSkinTypes.Any()) return (skinTypePercentages, new List<Service>());

            // Lấy danh sách ServiceRecommendation cho tất cả loại da có % cao nhất
            var recommendedServices = await _unitOfWork.Repository<ServiceRecommendation>()
                .ListAsync(sr => highestSkinTypes.Select(st => st.Id).Contains(sr.SkinTypeID), null);

            // Lấy danh sách dịch vụ (tránh trùng lặp)
            var serviceIds = recommendedServices.Select(sr => sr.ServiceID).Distinct().ToList();

            var services = await _unitOfWork.Repository<Service>()
                .ListAsync(s => serviceIds.Contains(s.Id), null);

            return (skinTypePercentages, services.ToList());

        }

    }
}
