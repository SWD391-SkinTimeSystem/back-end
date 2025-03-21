using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
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
        public ServiceRepository(ApplicationDbContext context) : base(context)
        {}

        public async Task CreateService(Service service, ICollection<Guid> SkintypeIds, ICollection<IFormFile> ServiceImages, ICollection<ServiceDetail> ServiceDetails)
        {
            var listURL = new List<string>();
            service.Id = Guid.NewGuid();

            //// Upload các hình ảnh và lưu URL
            //foreach (var file in ServiceImages)
            //{
            //    if (file.Length > 0)
            //    {
            //        string fileUrl = await _fileService.Upload(file);
            //        listURL.Add(fileUrl);
            //    }
            //}

            // Gán ID cho ServiceDetails
            foreach (var detail in ServiceDetails)
            {
                detail.ServiceID = service.Id;
            }

            // Gán ServiceDetails vào Service
            service.ServiceDetailNavigation = ServiceDetails;
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            foreach (var url in listURL)
            {
                _context.ServiceImages.Add(new ServiceImage
                {
                    ImageUrl = url,
                    ServiceId = service.Id
                });
            }

            foreach (var skinTypeId in SkintypeIds)
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

        public async Task<Service?> GetTretmenplan(Guid idService)
        {
            return await _context.Services
        .Include(s => s.ServiceDetailNavigation)
        .Include(s => s.ServiceImageNavigation)
        .Where(s => s.Id == idService && s.ServiceDetailNavigation.Count() >= 2)
        .FirstOrDefaultAsync();
        }
    }
}