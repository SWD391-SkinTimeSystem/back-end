using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task CreateService(Service service, ICollection<Guid> SkintypeIds, ICollection<IFormFile> ServiceImages, ICollection<ServiceDetail> ServiceDetails);
        Task<ICollection<Service>> GetAllTretmenplan();
        Task<Service> GetService(Guid idService);
        Task<Service> GetTretmenplan(Guid idService);
    }
}
