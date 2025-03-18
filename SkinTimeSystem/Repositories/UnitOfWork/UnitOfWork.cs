using BusinessObject.Entities;
using Repositories.Data;
using Repositories.Implement;
using Repositories.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        public IBookingRepository Bookings { get; private set; }
        public IServiceRepository Services { get; private set; }
        public IUserRepository UserRepository {get; private set; }

        public ITherapistRepository TherapistRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Bookings = new BookingRepository(context);
            Services = new ServiceRepository(context);
            UserRepository = new UserRepository(context);
            TherapistRepository = new TherapistRepository(context);
        }
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                var repository = new GenericRepository<T>(_context);
                _repositories[typeof(T)] = repository;
            }
            return (IGenericRepository<T>)_repositories[typeof(T)];
        }
    }
}
