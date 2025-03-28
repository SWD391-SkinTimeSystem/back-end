using AutoMapper;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.Category;
using Services.Commons.DTOs.Service;
using Services.Commons.DTOs.Skintype;
using Services.Commons.DTOs.SkinType;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class SkintypeService : ISkintypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SkintypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {               
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<bool>> CreateSkinType(SkinTypeCreationDTO skintype)
        {
            try
            {
                var result =await  _unitOfWork.Repository<SkinType>().AddAsync(_mapper.Map<SkinType>(skintype));
                await _unitOfWork.Complete();
                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failed(ServiceError.UnhandledException(ex.Message));
            }      
        }

        public async Task<ServiceResult<ICollection<SkintypeDetailDTO>>> GetAllSkintype()
        {
            var listSkintype = await _unitOfWork.Repository<SkinType>().GetAllAsync();
            var listSkintypeDTO = _mapper.Map<ICollection<SkintypeDetailDTO>>(listSkintype);

            return ServiceResult<ICollection<SkintypeDetailDTO>>.Success(listSkintypeDTO);
        }

        public async Task<ServiceResult<ICollection<SkinTypeDescriptionDTO>>> GetAllSkinTypeWithDescription()
        {
            var listSkintype = await _unitOfWork.Repository<SkinType>().GetAllAsync();
            var listSkintypeDTO = _mapper.Map<ICollection<SkinTypeDescriptionDTO>>(listSkintype);

            return ServiceResult<ICollection<SkinTypeDescriptionDTO>>.Success(listSkintypeDTO);
        }




        public Task<SkinType> GetSkinTypeById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UpdateSkinType(Guid id, SkinType skintype)
        {
            throw new NotImplementedException();
        }
    }
}
