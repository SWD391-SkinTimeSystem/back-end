using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enum;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.Therapist;
using Services.Interfaces;
using System.Linq.Expressions;

namespace Services.Implement
{
    public class TherapistService : ITherapistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TherapistService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                return ServiceResult<Guid>.Failed(ServiceError.UnhandledException(ex.Message));
            }
        }

        public async Task<PaginationResult<TherapistDTO>> GetAllTherapist(int page, int pageSize)
        {
            PaginationResult<Therapist> result = await _unitOfWork.Repository<Therapist>().AsPaginated(page, pageSize);

            return new PaginationResult<TherapistDTO>
            {
                Content = _mapper.Map<ICollection<TherapistDTO>>(result.Content),
                CurrentPage = page,
                ItemAmount = result.ItemAmount,
                PageSize = pageSize,
            };
        }

        public async Task<ServiceResult<TherapistDTO>> GetTherapistWithId(Guid id)
        {
            var therapist_info = await _unitOfWork.TherapistRepository.GetTherapistInformationWithId(id);

            if ( therapist_info == null)
            {
                return ServiceResult<TherapistDTO>.Failed(ServiceError.NotFound("Can not find therapist entity with provided Id"));
            }

            return ServiceResult<TherapistDTO>.Success(_mapper.Map<TherapistDTO>(therapist_info));
        }

        public async Task<PaginationResult<TherapistDTO>> GetAllTherapistWithStatus(int page, int pageSize, TherapistStatus status)
        {
            PaginationResult<Therapist> result = await _unitOfWork.Repository<Therapist>().AsPaginated(page, pageSize, x=> x.Status == status);

            return new PaginationResult<TherapistDTO>
            {
                Content = _mapper.Map<ICollection<TherapistDTO>>(result.Content),
                CurrentPage = page,
                ItemAmount = result.ItemAmount,
                PageSize = pageSize,
            };
        }

        public async Task<PaginationResult<TherapistDTO>> GetAvailableTherapist(int page, int pageSize)
        {
            PaginationResult result = await GetAllTherapist(page, pageSize, x => x.Status == TherapistStatus.Available);
            return new PaginationResult<TherapistDTO>
            {
                Content = _mapper.Map<ICollection<TherapistDTO>>(result.Content),
                CurrentPage = page,
                ItemAmount = result.ItemAmount,
                PageSize = pageSize
            };
        }

        public async Task<ICollection<TherapistDTO>> GetAvailableTherapist(DateOnly date, TimeOnly startTime, int duration)
        {
            TimeOnly endTime = startTime.AddMinutes(duration);

            // This is one of a hell thingamabob function, please do not touch.
            Expression<Func<Therapist, bool>> predicate = x => x.Status == TherapistStatus.Available && (!x.BookingNavigation
            .Any(b => b.ScheduleNavigation.Any(s =>s.Date == date && s.ReservedStartTime <= endTime && startTime <= s.ReservedEndTime)));

           

            ICollection<Therapist> result = await _unitOfWork.TherapistRepository.GetAllTherapistWithScheduleInformation(predicate);

            return _mapper.Map<ICollection<TherapistDTO>>(result);
        }

        public async Task<ServiceResult<TherapistDTO>> GetFirstAvailableTherapist(DateOnly date, TimeOnly startTime, int duration)
        {
            TherapistDTO? result = (await GetAvailableTherapist(date, startTime, duration)).FirstOrDefault();

            if (result == null)
            {
                return ServiceResult<TherapistDTO>.Failed(ServiceError.ValidationFailed("No available therapist found"));
            }

            return ServiceResult<TherapistDTO>.Success(result);
        }

        public Task<ServiceResult<Guid>> UpdateTherapist(Guid id, Therapist information)
        {
            throw new NotImplementedException();
        }

        private async Task<PaginationResult<TherapistDTO>> GetAllTherapist(int page, int pageSize, Expression<Func<Therapist, bool>> predicate)
        {
            PaginationResult result = await _unitOfWork.Repository<Therapist>().AsPaginated(page, pageSize, predicate);

            return new PaginationResult<TherapistDTO>
            {
                Content = _mapper.Map<ICollection<TherapistDTO>>(result),
                CurrentPage = page,
                ItemAmount = result.ItemAmount,
                PageSize = pageSize,
            };
        }
    }
}
