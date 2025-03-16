using BusinessObject.Entities;
using Services.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ServiceResult<bool>> SaveCategory(ServiceCategory serviceCategory);
        Task<ServiceResult<ICollection<Service>>> ListServiceByCategory(Guid id);

    }
}
