using Microsoft.EntityFrameworkCore;
using SkinTime.BLL.Commons;
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

        public Task<ServiceResult> CreateQuestion(Question question)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> CreateQuestions(ICollection<Question> questions)
        {
            // Validation
            if (questions.Any(x => x.QuestionOptionsNavigation.Count == 0))
            {
                return ServiceResult.Failed(ServiceError.ValidationFailed("There is questions with 0 choice"));
            }

            ICollection<Question> current =  await _unitOfWork.Repository<Question>().GetAllAsync();

            foreach (Question question in questions)
            {
                if (current.FirstOrDefault(q => q.Id == question.Id) != null)
                {
                    return ServiceResult.Failed(ServiceError.ValidationFailed("Can not add question with an existing id!"));
                }
                else
                {
                    // Add new question
                    await _unitOfWork.Repository<Question>().AddAsync(question);
                }
            }
            await _unitOfWork.Complete();
            return ServiceResult.Success();
        }

        public Task<ServiceResult> UpdateQuestion(Question questions)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> UpdateAllQuestion(ICollection<Question> questions)
        {
            if (questions.Any(x => x.QuestionOptionsNavigation.Count == 0))
            {
                return ServiceResult.Failed(ServiceError.ValidationFailed("There is questions with 0 choice"));
            }

            // WARNING: THIS WILL DELETE EVERY SINGLE RECORD OF QUESTION AND OPTIONS IN THE DATABASE!!!!
            foreach (var question in await _unitOfWork.Repository<Question>().GetAllAsync())
            {
                _unitOfWork.Repository<Question>().Delete(question);
            }

            // Add new records to the database
            foreach (var question in questions)
            {
                await _unitOfWork.Repository<Question>().AddAsync(question);
            }

            await _unitOfWork.Complete();

            return ServiceResult.Success();
        }

        public Task<ServiceResult> DeleteQuestion(Question questions)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Question>> GetAllQuestion() =>_unitOfWork.Repository<Question>().GetAllAsync(q => q.QuestionOptionsNavigation);
        

        public async Task<(Dictionary<SkinType, double> SkinTypes, List<Service> Services)> GetServiceRecommments(List<Guid> listResult)
        {

            var allSkinTypes = await _unitOfWork.Repository<SkinType>().ListAsync();

            var questionOptions = await _unitOfWork.Repository<QuestionOption>()
                //.ListAsync(qo => listResult.Contains(qo.Id), null, qo => qo.Include(q => q.SkinType));
                .ListAsync(x => x.Include(x => x.SkinTypeNavigation), qo => listResult.Contains(qo.Id));

            if (!questionOptions.Any())
            {
                return (allSkinTypes.ToDictionary(st => st, st => 0.0), new List<Service>());
            }

            var skinTypeCounts = questionOptions
                .GroupBy(qo => qo.SkinTypeNavigation)
                .ToDictionary(g => g.Key, g => g.Count()); 

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
