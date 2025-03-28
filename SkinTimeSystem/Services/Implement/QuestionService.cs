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

        public async Task<ICollection<Question>> GetAllQuestion()=> await _unitOfWork.QuestionRepository.GetAllQuestionsAsync();
        



        public async Task<(Dictionary<SkinType, double> SkinTypes, List<Service> Services)> GetServiceRecommments(List<Guid> listResult)
        {

            var skinTypePercentages = await _unitOfWork.QuestionRepository.GetSkinTypePercentagesAsync(listResult);

            var maxPercentage = skinTypePercentages.Max(st => st.Value);
            var highestSkinTypes = skinTypePercentages
                .Where(st => st.Value == maxPercentage)
                .Select(st => st.Key)
                .ToList();

            if (!highestSkinTypes.Any()) return (skinTypePercentages, new List<Service>());

            var recommendedServices = await _unitOfWork.Repository<Service>()
                .ListAsync(s => s.SkinTypes.Any(st => highestSkinTypes.Select(hst => hst.Id).Contains(st.Id)), null);

            return (skinTypePercentages, recommendedServices.ToList());
        }

        public Task<ServiceResult> UpdateAllQuestion(ICollection<QuestionCreationDTO> questions)
        {
            throw new NotImplementedException();
        }
    }
}
