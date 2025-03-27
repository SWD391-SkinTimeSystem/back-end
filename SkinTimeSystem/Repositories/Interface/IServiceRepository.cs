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
        Task<Guid> CreateServiceBasic(Service service, ICollection<Guid> SkintypeIds, ICollection<ServiceDetail> ServiceDetails);
        Task CreateServiceAdvand(Guid idService, IFormFile? thumbnail, ICollection<IFormFile>? serviceImages);
        Task<ICollection<Service>> GetAllTretmenplan();
        Task<PaginationResult<Service>> GetAllService(string? searchKey, int page , int pageSize );
        Task<PaginationResult<Service>> GetAllServiceAvailibeEdit(string? searchKey, int page, int pageSize );
        Task<Service> GetService(Guid idService);
        Task<Service> GetTretmenplan(Guid idService);
    }
}
