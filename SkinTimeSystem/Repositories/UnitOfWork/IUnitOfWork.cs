using BusinessObject.Entities;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBookingRepository Bookings { get; }
        IServiceRepository Services { get; }
        IUserRepository UserRepository { get; }
        ITherapistRepository TherapistRepository { get; }
        IEventRepository EventRepository { get; }

        IGenericRepository<TEntity> Repository<TEntity>()
           where TEntity : BaseEntity;

        Task<int> Complete();
    }
}
