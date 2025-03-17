using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Repositories.UnitOfWork;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.SqlServer.Server;
using StackExchange.Redis;
using Services.FileSetting;
using Services.Commons;

namespace Services.Implement
{
    public class SkinTimeService : ISkinTimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileService _fileService;

        public SkinTimeService(FileService fileService,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<ServiceResult<bool>> CreateService(Service service, ICollection<IFormFile> serviceImages, ICollection<Guid> skintypeIds)
        {
            var listURL = new List<string>();
            try
            {
                foreach (var file in serviceImages)
                {
                    if (file.Length > 0)
                    {
                        string fileUrl = await _fileService.Upload(file);
                        listURL.Add(fileUrl);
                    }
                }
                await _unitOfWork.Services.CreateService(service, listURL, skintypeIds);
                return ServiceResult<bool>.Success(true);
            }

            catch (Exception ex)
            {
                return ServiceResult<bool>.Failed(new ServiceError("UploadFailed", $"Failed to upload images: {ex.Message}"));
            }

        }

        public async Task<ICollection<Service>> GetAllService() => await _unitOfWork.Repository<Service>().GetAllAsync();

        public async Task<ICollection<Service>> GetAllTreatmentplant()
        {
           var listService = await _unitOfWork.Services.GetAllTretmenplan();
            return  listService;
        }

        public async Task<(Service?, List<(Booking?, Feedback?, User?)>?)> GetService(Guid idService)
        {
            var service = await _unitOfWork.Repository<Service>()
                .GetByConditionAsync(s => s.Id == idService,
                    query => query.Include(s => s.ServiceDetailNavigation)
                                  .Include(s => s.ServiceImageNavigation));

            var bookings = await _unitOfWork.Repository<Booking>()
                .ListAsync(
                    filter: b => b.ServiceId == idService,
                    orderBy: null,
                    includeProperties: query => query
                        .Include(b => b.CustomerNavigation)
                        .Include(b => b.FeedbackNavigation!)
                );

            var result = bookings
                .Where(b => b.FeedbackNavigation != null)
                .Select(b => (b, b.FeedbackNavigation!, b.CustomerNavigation))
                .ToList();

            return (service, result.Any() ? result : null);
        }


        public async Task<Service?> GetTreatmentplant(Guid idService)
        {
            return await _unitOfWork.Repository<Service>()
                .GetByConditionAsync(
                    s => s.Id == idService,
                    includeProperties: query => query.Include(s => s.ServiceDetailNavigation)
                );
        }


    }


}

