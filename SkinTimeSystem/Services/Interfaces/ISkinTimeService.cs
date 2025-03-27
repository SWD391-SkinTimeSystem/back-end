using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Repositories;
using Services.Commons;
using Services.Commons.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISkinTimeService
    {
        Task<ServiceResult<ServiceDTO>> GetService(Guid idService);
        Task<ServiceResult<ServiceDTO>> GetTreatmentplant(Guid idService);
        Task<ServiceResult<ICollection<ServiceDTO>>> GetAllTreatmentplant();
        Task<PaginationResult<ServiceDTO>> GetAllService(string? searchKey, int page, int pageSize);
        Task<PaginationResult<ServiceDTO>> GetAllServiceAvailibeEdit(string? searchKey, int page, int pageSize);
        Task<ServiceResult<bool>> CreateServiceAdvand(ServiceCreateAdvandDTO serviceDTO);
        Task<ServiceResult<Guid>> CreateServiceBasic(ServiceCreateBasicDTO serviceDTO);
    }
}
