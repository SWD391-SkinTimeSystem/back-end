using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Services.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISkinTimeService
    {
        Task<(Service?, List<(Booking?, Feedback?, User?)>?)> GetService(Guid idService);
        Task<Service> GetTreatmentplant(Guid idService);
        Task<ICollection<Service>> GetAllTreatmentplant();
        Task<ICollection<Service>> GetAllService();
        Task<ServiceResult<bool>> CreateService(Service service,ICollection<IFormFile> serviceImages, ICollection<Guid> skintypeIds);
    }
}
