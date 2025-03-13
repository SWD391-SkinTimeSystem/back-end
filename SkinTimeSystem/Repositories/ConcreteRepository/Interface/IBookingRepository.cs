using BusinessObject.Entities;
using BusinessObject.Enum;
using Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ConcreteRepository.Interface
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<ICollection<Booking>> GetAppointments(Guid userId, string status);
    }
}
