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
using System.Reflection.Metadata.Ecma335;

namespace Services.Implement
{
    public class SkinTimeService : ISkinTimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly FileService _fileService;

        public SkinTimeService(FileService fileService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<ServiceResult<bool>> CreateService(ServiceCreateDTO serviceDTO)
        {
                var service = _mapper.Map<Service>(serviceDTO);
                var servicedetails = _mapper.Map<ICollection<ServiceDetail>>(serviceDTO.ServiceDetails);
                await _unitOfWork.Services.CreateService(service,serviceDTO.SkintypeIds, serviceDTO.ServiceImages, servicedetails);
                return ServiceResult<bool>.Success(true);

        }

        public async Task<ServiceResult<ICollection<ServiceDTO>>> GetAllService() { 
           var listService = await _unitOfWork.Repository<Service>().GetAllAsync();
            return ServiceResult<ICollection<ServiceDTO>>.Success(_mapper.Map<ICollection<ServiceDTO>>(listService));
        }
        public async Task<ServiceResult<ICollection<ServiceDTO>>> GetAllTreatmentplant()
        {
            var listTreatmentPlan = await _unitOfWork.Services.GetAllTretmenplan();
            return ServiceResult<ICollection<ServiceDTO>>.Success(_mapper.Map<ICollection<ServiceDTO>>(listTreatmentPlan));
        }



        public async Task<ServiceResult<ServiceDTO>> GetService(Guid idService) {

            var service = await _unitOfWork.Services.GetService(idService);
            return ServiceResult<ServiceDTO>.Success(_mapper.Map<ServiceDTO>(service));
        }


        public async Task<ServiceResult<ServiceDTO>> GetTreatmentplant(Guid idService){  
            var treatmentPlant = await _unitOfWork.Services.GetTretmenplan(idService);
            return ServiceResult<ServiceDTO>.Success(_mapper.Map<ServiceDTO>(treatmentPlant));
        }
    }


}

