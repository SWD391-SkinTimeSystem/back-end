using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.FeedbackService
{
    public interface IFeedbackService
    {
        Task<ServiceResult<Feedback>> CreateNewFeedback(Feedback feedback);

        Task<ServiceResult<Feedback>> GetFeedbackWithId(string feedbackId);

        Task<ServiceResult<ICollection<Feedback>>> GetAllFeedback();

        Task<ServiceResult<ICollection<Feedback>>> GetAllUserFeedback(string userId);

        Task<ServiceResult<Feedback>> GetBookingFeedback(string bookingId);

        Task<ServiceResult<ICollection<Feedback>>> GetAllServiceFeedback(string serviceId);

        Task<ServiceResult<ICollection<Feedback>>> GetAllTherapistFeedback(string therapistId);
    }
}
