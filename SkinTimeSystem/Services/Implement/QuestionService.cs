using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.Question;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
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
            if (questions.Any(x => x.QuestionOptionsNavigation.Count == 0))
            {
                return ServiceResult.Failed(ServiceError.ValidationFailed("There is questions with 0 choice"));
            }

            ICollection<Question> current = await _unitOfWork.Repository<Question>().GetAllAsync();

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

        public async Task<ICollection<Question>> GetAllQuestion()
        {
            var item = (await _unitOfWork.Repository<Question>()
            .ListAsync(includeProperties: q => q.Include(q => q.QuestionOptionsNavigation).ThenInclude(x => x.SkinTypes)));

            return item.ToList();
        }


        public async Task<(Dictionary<SkinType, double> SkinTypes, List<Service> Services)> GetServiceRecommments(List<Guid> listResult)
        {

            var allSkinTypes = await _unitOfWork.Repository<SkinType>().ListAsync();

            var questionOptions = await _unitOfWork.Repository<QuestionOption>()
    .ListAsync(
        x => x.Include(qo => qo.SkinTypes), 
        qo => listResult.Contains(qo.Id)
    );



            if (!questionOptions.Any())
            {
                return (allSkinTypes.ToDictionary(st => st, st => 0.0), new List<Service>());
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
              : 0.0);


            var maxPercentage = skinTypePercentages.Max(st => st.Value); // Tìm % cao nhất

            var highestSkinTypes = skinTypePercentages
                .Where(st => st.Value == maxPercentage) // Lấy tất cả loại da có cùng % cao nhất
                .Select(st => st.Key)
                .ToList();

            if (!highestSkinTypes.Any()) return (skinTypePercentages, new List<Service>());
            var recommendedServices = await _unitOfWork.Repository<Service>()
    .ListAsync(s => s.SkinTypes.Any(st => highestSkinTypes.Select(hst => hst.Id).Contains(st.Id)), null);


            var serviceIds = recommendedServices.Select(s => s.Id).Distinct().ToList();


            var services = await _unitOfWork.Repository<Service>()
                .ListAsync(s => serviceIds.Contains(s.Id), null);

            return (skinTypePercentages, services.ToList());

        }

        public Task<ServiceResult> UpdateAllQuestion(ICollection<QuestionCreationDTO> questions)
        {
            throw new NotImplementedException();
        }
    }
}
