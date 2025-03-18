using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.EventEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Repositories;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.Event;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<ServiceResult> CancelEvent(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> CreateNewEvent(EventCreationDTO eventInformation)
        {
            // Find for any event that's clashing with the new event time.
            var clashingEvent = await _unitOfWork.Repository<Event>()
                .FindAsync(x => x.EventDate == eventInformation.Date && x.TimeStart <= eventInformation.EndTime && x.TimeEnd >= eventInformation.StartTime
                && (x.Status == EventStatus.Approved || x.Status == EventStatus.OnGoing));

            if (clashingEvent != null)
            {
                return ServiceResult.Failed(ServiceError.ValidationFailed("There is another event curretnly existing in the selected time!"));
            }

            Event eventEntity = _mapper.Map<Event>(eventInformation);
            eventEntity.Id = Guid.NewGuid();

            eventEntity = await _unitOfWork.Repository<Event>().AddAsync(eventEntity);
            await _unitOfWork.Complete();

            return ServiceResult<EventDTO>.Success(_mapper.Map<EventDTO>(eventEntity));
        }

        public async Task<ServiceResult> DeleteEvent(Guid id)
        {
            var result = await _unitOfWork.EventRepository.GetByIdAsync(id);

            if (result == null || result.Status == EventStatus.Removed)
            {
                return ServiceResult.Failed(ServiceError.NotFound("can not find the required event"));
            }

            if (result.Status != EventStatus.ApprovePending)
            {
                return ServiceResult.Failed(ServiceError.ValidationFailed("Can not remove an event that has been approved"));
            }
            
            result.Status = EventStatus.Removed;
            _unitOfWork.EventRepository.Update(result);

            return ServiceResult.Success(result.Id);
        }

        public async Task<ICollection<EventDTO>> GetAllEvents()
        {
            return _mapper.Map<ICollection<EventDTO>>(await _unitOfWork.EventRepository.GetAllEvents());
        }

        public async Task<PaginationResult<AvailableEventDTO>> GetAvailableEventList(int page, int pageSize)
        {
            Expression<Func<Event, bool>> filter = x => x.TicketNavigation.Count() < x.Capacity && x.Status == EventStatus.Approved;

            PaginationResult<Event> results = await _unitOfWork.EventRepository.GetEventPaginated(page, pageSize, filter);

            Console.WriteLine(results.ItemAmount);

            return new PaginationResult<AvailableEventDTO>
            {
                Content = _mapper.Map<ICollection<AvailableEventDTO>>(results.Content),
                CurrentPage = results.CurrentPage,
                ItemAmount = results.ItemAmount,
                PageSize = pageSize
            };
        }

        public async Task<ServiceResult<ICollection<Event>>> GetEventByStatus(EventStatus eventStatus)
        {
            return ServiceResult<ICollection<Event>>
                .Success((await _unitOfWork.Repository<Event>()
                    .ListAsync(x => x.Include(b => b.TicketNavigation), filter: x => x.Status == eventStatus)).ToList());
        }

        public async Task<ServiceResult<ICollection<Event>>> GetEventList()
        {
            return ServiceResult<ICollection<Event>>
                .Success((await _unitOfWork.Repository<Event>()
                    .ListAsync(x => x.Include(b => b.TicketNavigation))).ToList());
        }

        public async Task<PaginationResult<EventDTO>> GetEventList(int page, int pageSize, Expression<Func<Event, bool>> expression)
        {
            PaginationResult<Event> result = await _unitOfWork.EventRepository.GetEventPaginated(page, pageSize, expression);

            return new PaginationResult<EventDTO>
            {
                Content = _mapper.Map<ICollection<EventDTO>>(result.Content),
                CurrentPage = page,
                ItemAmount = result.ItemAmount,
                PageSize = pageSize
            };
        }

        public async Task<PaginationResult<EventDTO>> GetEventListWithStatus(int page, int pageSize, EventStatus status)
        {
            PaginationResult<Event> result = await _unitOfWork.EventRepository.GetEventWithStatusPaginated(page, pageSize, status);

            return new PaginationResult<EventDTO>
            {
                Content = _mapper.Map<ICollection<EventDTO>>(result.Content),
                CurrentPage = page,
                ItemAmount = result.ItemAmount,
                PageSize = pageSize
            };
        }

        public Task<ServiceResult> UpdateEvent(Guid eventId, EventUpdateDTO eventInformation)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> UpdateEventStatus(Guid eventId, EventStatus status)
        {
            var target = await _unitOfWork.Repository<Event>().GetByIdAsync(eventId);

            if (target == null)
            {
                return ServiceResult.Failed(ServiceError.ValidationFailed("Can not find the event with provided id"));
            }

            if (target.Status == EventStatus.Removed)
            {
                return ServiceResult.Failed(ServiceError.ValidationFailed("Invalid operation!"));
            }

            target.Status = status;
            await _unitOfWork.Complete();


            return ServiceResult.Success(target);
        }

        public async Task<ServiceResult<EventDTO>> GetEventWithId(Guid id)
        {
            var result = await _unitOfWork.EventRepository.GetEventById(id);

            if (result == null)
            {
                return ServiceResult<EventDTO>.Failed(ServiceError.ValidationFailed("Can not find event with the provided id"));
            }

            return ServiceResult<EventDTO>.Success(_mapper.Map<EventDTO>(result));
        }
    }
}
