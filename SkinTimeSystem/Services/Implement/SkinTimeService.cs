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
using Services.Commons;
using Services.Commons.DTOs.Service;
using AutoMapper;
using System.Reflection.Metadata.Ecma335;
using Repositories;
using Services.Commons.DTOs.Event;
using System.Linq.Expressions;

namespace Services.Implement
{
    public class SkinTimeService : ISkinTimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SkinTimeService( IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<bool>> CreateServiceAdvand(ServiceCreateAdvandDTO serviceDTO)
        {
            try
            {
                await _unitOfWork.Services.CreateServiceAdvand(serviceDTO.IdService, serviceDTO.Thumbnail, serviceDTO.ServiceImages);
                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failed(ServiceError.UnhandledException(ex.Message));
            }
        }

        public async Task<ServiceResult<Guid>> CreateServiceBasic(ServiceCreateBasicDTO serviceDTO)
        {
            try
            {
                var service = _mapper.Map<Service>(serviceDTO);
                var servicedetails = _mapper.Map<ICollection<ServiceDetail>>(serviceDTO.ServiceDetails);
                var result = await _unitOfWork.Services.CreateServiceBasic(service, serviceDTO.SkintypeIds, servicedetails);
                return ServiceResult<Guid>.Success(result);
            }
            catch (Exception ex)
            {
                return ServiceResult<Guid>.Failed(ServiceError.UnhandledException(ex.Message));
            }
        }


        public async Task<PaginationResult<ServiceDTO>> GetAllService(string? searchKey, int page, int pageSize )
        {
            PaginationResult<Service> listService = await _unitOfWork.Services.GetAllService(searchKey, page, pageSize);

            return new PaginationResult<ServiceDTO>
            {
                Content = _mapper.Map<List<ServiceDTO>>(listService.Content), 
                CurrentPage = listService.CurrentPage,
                ItemAmount = listService.ItemAmount,
                PageSize = pageSize
            };
        }

        public async Task<PaginationResult<ServiceDTO>> GetAllServiceAvailibeEdit(string? searchKey, int page, int pageSize)
        {
            PaginationResult<Service> listService = await _unitOfWork.Services.GetAllServiceAvailibeEdit(searchKey, page, pageSize);

            return new PaginationResult<ServiceDTO>
            {
                Content = _mapper.Map<List<ServiceDTO>>(listService.Content),
                CurrentPage = listService.CurrentPage,
                ItemAmount = listService.ItemAmount,
                PageSize = pageSize
            };
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

