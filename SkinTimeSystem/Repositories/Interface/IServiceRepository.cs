using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task CreateService(Service service, List<string> listURl, ICollection<Guid> skinTypeIds);
        Task<ICollection<Service>> GetAllTretmenplan();
        Task<Service> GetService(Guid idService);
        Task<Service> GetTretmenplan(Guid idService);
    }
}
