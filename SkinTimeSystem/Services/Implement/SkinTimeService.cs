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
using Services.Commons.DTOs.Service;
using AutoMapper;

namespace Services.Implement
{
    public class SkinTimeService : ISkinTimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly FileService _fileService;

        public SkinTimeService(FileService fileService,IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _mapper = mapper;
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

        public async Task<ICollection<Service>> GetAllTreatmentplant() => await _unitOfWork.Services.GetAllTretmenplan();

        public async Task<ServiceResult<ServiceDTO>> GetService(Guid idService) { 
           
        var service =   await _unitOfWork.Services.GetService(idService);
            return ServiceResult<ServiceDTO>.Success(_mapper.Map<ServiceDTO>(service));
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

