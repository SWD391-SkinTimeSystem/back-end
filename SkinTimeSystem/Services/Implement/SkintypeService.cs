using AutoMapper;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.Category;
using Services.Commons.DTOs.Skintype;
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
        public async Task<ServiceResult<ICollection<SkintypeDetailDTO>>> GetAllSkintype()
        {
            var listSkintype = await _unitOfWork.Repository<SkinType>().GetAllAsync();
            var listSkintypeDTO = _mapper.Map<ICollection<SkintypeDetailDTO>>(listSkintype);

            return ServiceResult<ICollection<SkintypeDetailDTO>>.Success(listSkintypeDTO);
        }
    }
}
