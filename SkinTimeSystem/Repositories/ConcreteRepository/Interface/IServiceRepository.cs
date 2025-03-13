using BusinessObject.Entities;
using Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ConcreteRepository.Interface
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task CreateService(Service service,List<string> listURl,ICollection<Guid> skinTypeIds);
    }
}
