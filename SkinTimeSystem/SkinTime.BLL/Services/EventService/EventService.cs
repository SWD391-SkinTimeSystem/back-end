using Microsoft.EntityFrameworkCore;
using SkinTime.DAL.Entities;
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

        public async Task<Event?> CreateNewEvent(Event eventInformation)
        {
            // Find for any event that's clashing with the new event time.
            var clashingEvent = await _unitOfWork.Repository<Event>()
                .FindAsync(x => x.TimeStart < eventInformation.TimeEnd && x.TimeEnd > eventInformation.TimeStart);
        
            if (clashingEvent != null)
            {
                // TODO: Add a way to return the failure message to the controller!
                Console.WriteLine("An existing event found to have clashing time with the current event.");
                return null;
            }

            await _unitOfWork.Repository<Event>().AddAsync(eventInformation);

            await _unitOfWork.Complete();

            return eventInformation;
        }

        public Task<Event> DeleteEvent(Guid eventId)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Event>> GetEventList()
        {
            return await _unitOfWork.Repository<Event>()
                .GetAll()
                .ToListAsync();
        }

        public async Task<ICollection<Event>> GetEventList(Expression<Func<Event, bool>> expression)
        {
            return await _unitOfWork.Repository<Event>()
                .GetAll()
                .Where(expression)
                .ToListAsync();
        }

        public async Task<Event?> GetEventWithId(Guid id)
        {
            return await _unitOfWork.Repository<Event>()
                .GetEntityByIdAsync(id);
        }

        public Task<Event> UpdateEvent(Guid eventId, Event eventInformation)
        {
            throw new NotImplementedException();
        }
    }
}
