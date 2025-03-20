using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext context) : base(context) { }

        public async Task CreateService(Service service, List<string> imageUrls, ICollection<Guid> skinTypeIds)
        {
            service.Id = Guid.NewGuid();

            foreach (var detail in service.ServiceDetailNavigation)
            {
                detail.ServiceID = service.Id;
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            foreach (var url in imageUrls)
            {
                _context.ServiceImages.Add(new ServiceImage
                {
                    ImageUrl = url,
                    ServiceId = service.Id
                });
            }

            foreach (var skinTypeId in skinTypeIds)
            {
                _context.ServiceRecommendation.Add(new ServiceRecommendation
                {
                    ServiceID = service.Id,
                    SkinTypeID = skinTypeId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Service>> GetAllTretmenplan()
        {

            var services = await _context.Services
     .Where(s => s.ServiceDetailNavigation.Count > 2)
     .Include(s => s.ServiceDetailNavigation)
     .ToListAsync();
            return services;
        }

        public async Task<Service?> GetService(Guid idService)
        {
            return await _context.Services
                .Include(s => s.ServiceDetailNavigation)
                .Include(s => s.ServiceImageNavigation)
                .FirstOrDefaultAsync(s => s.Id == idService);
        }

    }
}