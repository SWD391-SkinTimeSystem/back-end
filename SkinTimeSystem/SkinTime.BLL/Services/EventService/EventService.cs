using Microsoft.EntityFrameworkCore;
using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum.EventEnums;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ServiceResult> CancelEvent(string eventId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<Event>> CreateNewEvent(Event eventInformation)
        {
            // Find for any event that's clashing with the new event time.
            var clashingEvent = await _unitOfWork.Repository<Event>()
                .FindAsync(x => x.TimeStart <= eventInformation.TimeEnd && x.TimeEnd >= eventInformation.TimeStart 
                && (x.Status != EventStatus.Canceled || x.Status != EventStatus.Removed || x.Status != EventStatus.Removed));
        
            if (clashingEvent != null)
            {
                return ServiceResult<Event>
                    .Failed(ServiceError.ValidationFailed("There is another event curretnly existing in the selected time!"));
            }

            eventInformation.Id = Guid.NewGuid();
            Event result = await _unitOfWork.Repository<Event>().AddAsync(eventInformation);
            await _unitOfWork.Complete();

            return ServiceResult<Event>.Success(result);
        }

        public async Task<ServiceResult> DeleteEvent(string eventId)
        {
            if (Guid.TryParse(eventId, out var parsedId))
            {
                var result = await _unitOfWork.Repository<Event>().GetByIdAsync(parsedId);

                if (result == null || result.Status == EventStatus.Removed)
                {
                    return ServiceResult.Failed(ServiceError.NotFound("can not find the required service"));
                }

                // Business logic for and event deletion and refunds
            throw new NotImplementedException();
        }

            return ServiceResult.Failed(ServiceError.ValidationFailed("The given id does not match the correct format"));
        }

        public Task<ServiceResult<Event>> GetEventByStatus(EventStatus eventStatus)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<ICollection<Event>>> GetEventList()
        {
            return ServiceResult<ICollection<Event>>
                .Success((await _unitOfWork.Repository<Event>()
                    .ListAsync(x => x.Include(b => b.TicketNavigation))).ToList());
        }
             
        public async Task<ServiceResult<ICollection<Event>>> GetEventList(Expression<Func<Event, bool>> expression)
        {
            return ServiceResult<ICollection<Event>>
                .Success((await _unitOfWork.Repository<Event>().ListAsync(expression)).ToList());
        }

        public async Task<ServiceResult<Event>> GetEventWithId(Guid id)
        {
            var result = await _unitOfWork.Repository<Event>().GetByIdAsync(id, x => x.Include( x =>
            x.TicketNavigation));

            if (result == null)
        {
                return ServiceResult<Event>.Failed(ServiceError.ValidationFailed("Can not find event with the provided id"));
            }

            return ServiceResult<Event>.Success(result);
        }

        public Task<ServiceResult<Event>> UpdateEvent(Guid eventId, Event eventInformation)
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
    }
}
