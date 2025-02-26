using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.SkinTimeService
{
    public interface ISkinTimeService
    {
        Task<(Service?, List<(Booking?, Feedback?, User?)>?)> GetService(Guid idService);
        Task<Service> GetTrementplant(Guid idService);
        Task<ICollection<Service>> GetAllService();
    }
}
