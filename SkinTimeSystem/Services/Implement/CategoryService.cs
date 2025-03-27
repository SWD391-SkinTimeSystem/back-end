using AutoMapper;
using BusinessObject.Entities;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.Category;
using Services.Commons.DTOs.Service;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ICollection<CategoryDetailDTO>>> GetAllCategory()
        {
            var listCategory = await _unitOfWork.Repository<ServiceCategory>().GetAllAsync();
            var listCategoryDTO = _mapper.Map<ICollection<CategoryDetailDTO>>(listCategory);

            return ServiceResult<ICollection<CategoryDetailDTO>>.Success(listCategoryDTO);
        }



        public async Task<ServiceResult<ICollection<ServiceDTO>>> ListServiceByCategory(Guid id)
        {
            try
            {
                var services = await _unitOfWork.Repository<Service>().ListAsync(s => s.ServiceCategoryID == id);
                var listService = _mapper.Map<ICollection<ServiceDTO>>(services);
                return ServiceResult<ICollection<ServiceDTO>>.Success(listService);
            }
            catch (Exception ex)
            {
                return ServiceResult<ICollection<ServiceDTO>>.Failed(ServiceError.UnhandledException(ex.Message));
            }
        }


        public async Task<ServiceResult<bool>> SaveCategory(ServiceCategory serviceCategory)
        {
            try
            {
                if (serviceCategory.Id == null)
                {
                    serviceCategory.Id = Guid.NewGuid();
                    await _unitOfWork.Repository<ServiceCategory>().AddAsync(serviceCategory);
                }
                else
                {
                     _unitOfWork.Repository<ServiceCategory>().Update(serviceCategory);
                }
                await _unitOfWork.Complete();
                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failed(ServiceError.UnhandledException(ex.Message));
            }
           
        }
    }
    
}
