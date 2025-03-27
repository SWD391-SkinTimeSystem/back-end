using BusinessObject.Entities;
using BusinessObject.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Interface;
using SharedLibrary.FIleSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly FirebaseStorageService _fileService;
        public ServiceRepository(ApplicationDbContext context, FirebaseStorageService fileService) : base(context)
        {
            _fileService = fileService;
        }
        public async Task<PaginationResult<Service>> GetAllService(string? searchKey, int page = 1, int pageSize = 12)
        {
            Expression<Func<Service, bool>> filter = x => string.IsNullOrWhiteSpace(searchKey) ||
                                                          x.ServiceName.Contains(searchKey);
                                                          

            return await AsPaginated(
                page,
                pageSize,
                filter,
                includes: x => x.Include(x => x.ServiceDetailNavigation)
                                .Include(x => x.ServiceImageNavigation),
                order: x => x.OrderBy(x => x.CreatedTime)
            );
        }

        public async Task CreateServiceAdvand(Guid idService, IFormFile? thumbnail, ICollection<IFormFile>? serviceImages)
        {
            var service = await _context.Services.FindAsync(idService);

                service.Thumbnail = await _fileService.Upload(thumbnail);
       

            var imageEntities = new List<ServiceImage>();

            if (serviceImages != null && serviceImages.Any())
            {
                foreach (var file in serviceImages)
                {
                    if (file.Length > 0)
                    {
                        string fileUrl = await _fileService.Upload(file);
                        imageEntities.Add(new ServiceImage
                        {
                            ImageUrl = fileUrl,
                            ServiceId = service.Id
                        });
                    }
                }
                if (imageEntities.Count > 0)
                {
                    _context.ServiceImages.AddRange(imageEntities);
                }
            }
            await _context.SaveChangesAsync();
        }


        public async Task<Guid> CreateServiceBasic(Service service, ICollection<Guid> skintypeIds, ICollection<ServiceDetail> serviceDetails)
        {
            service.Id = Guid.NewGuid();
            service.Status = ServiceStatus.Available;
            service.Duration = serviceDetails.Sum(detail => detail.Duration);

            foreach (var detail in serviceDetails)
            {
                detail.ServiceID = service.Id;
            }

            service.ServiceDetailNavigation = serviceDetails.ToList();

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            var skinTypes = await _context.SkinTypes
                                 .Where(st => skintypeIds.Contains(st.Id))
                                 .ToListAsync();
            service.SkinTypes = skinTypes;

            return service.Id;
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

        public async Task<PaginationResult<Service>> GetAllServiceAvailibeEdit(string? searchKey, int page, int pageSize)
        {
            Expression<Func<Service, bool>> filter = x =>
         (string.IsNullOrWhiteSpace(searchKey) || x.ServiceName.Contains(searchKey)) &&
         !x.BookingNavigation.Any(b => b.Status == BookingStatus.NotStarted || b.Status == BookingStatus.Doing);


            return await AsPaginated(
                page,
                pageSize,
                filter,
                includes: x => x.Include(x => x.ServiceDetailNavigation)
                                .Include(x => x.ServiceImageNavigation),
                order: x => x.OrderBy(x => x.CreatedTime)
            );
        }
    }
}