using Microsoft.EntityFrameworkCore;
using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.DAL.Interfaces;
using System.Linq.Expressions;

namespace SkinTime.BLL.Services.TherapistService
{
    public class TherapistService : ITherapistService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TherapistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<Guid>> AddTherapist(Therapist therapist)
        {
            try
            {
                Therapist result = await _unitOfWork.Repository<Therapist>().AddAsync(therapist);
                await _unitOfWork.Complete();

                return ServiceResult<Guid>.Success(result.Id);
            }
            catch (DbUpdateException ex) 
            {
                Console.WriteLine($"{ex.Message}\nInnerException:{ex.InnerException?.Message}\n{ex.GetType()}:{ex.StackTrace}");
                return ServiceResult<Guid>.Failed(ServiceError.UnhandledException(ex.Message));
            }
        }

        public async Task<ServiceResult<ICollection<Therapist>>> GetAllTherapist(Expression<Func<Therapist, bool>> predicate)
        {
            IEnumerable<Therapist> result = await _unitOfWork.Repository<Therapist>().ListAsync(predicate);

            return ServiceResult<ICollection<Therapist>>.Success(result.ToList());
        }

        public async Task<ServiceResult<ICollection<Therapist>>> GetAllTherapist()
        {
            IEnumerable<Therapist> result = await _unitOfWork.Repository<Therapist>().ListAsync();

            return ServiceResult<ICollection<Therapist>>.Success(result.ToList());
        }

        public async Task<ServiceResult<ICollection<Therapist>>> GetAvailableTherapist()
        {
            IEnumerable<Therapist> result = await _unitOfWork.Repository<Therapist>()
                .ListAsync(x => x.Status == TherapistStatus.Available);

            return ServiceResult<ICollection<Therapist>>.Success(result.ToList());
        }

        public async Task<ServiceResult<ICollection<Therapist>>> GetAvailableTherapist(DateOnly date, TimeOnly startTime, int duration)
        {

            TimeOnly endTime = startTime.AddMinutes(duration);
            
            // This is one of a hell thingamabob function, please do not touch.
            Expression<Func<Therapist, bool>> predicate = x => x.Status == TherapistStatus.Available
            && ( x.BookingNavigation.Count == 0 || x.BookingNavigation
                .Any(b => b.ScheduleNavigation
                    .Any(s => s.ReservedStartTime <= endTime && startTime <= s.ReservedEndTime)));

            IEnumerable<Therapist> result = await _unitOfWork.Repository<Therapist>().ListAsync(x => x
            .Include(b => b.UserNavigation)
            .Include(b => b.BookingNavigation).ThenInclude(b => b.FeedbackNavigation)
            .Include(b => b.BookingNavigation).ThenInclude(b => b.ScheduleNavigation)
            .Include(b => b.CertificationNavigation), predicate);

            return ServiceResult<ICollection<Therapist>>.Success(result.ToList());
        }

        public async Task<ServiceResult<Therapist>> GetTherapistWithId(Guid id)
        {
            Therapist? result = await _unitOfWork.Repository<Therapist>()
                .GetByIdAsync(id, x => x
                    .Include(b => b.UserNavigation)
                    .Include(b => b.BookingNavigation).ThenInclude(x => x.FeedbackNavigation)
                    .Include(b => b.CertificationNavigation));

            if (result != null)
            {
                return ServiceResult<Therapist>.Success(result);
            }

            return ServiceResult<Therapist>.Failed(ServiceError.NotFound("Can not find therapist entity with provided Id"));
        }

        public Task<ServiceResult<Guid>> UpdateTherapist(Guid id, Therapist information)
        {
            throw new NotImplementedException();
        }
    }
}
