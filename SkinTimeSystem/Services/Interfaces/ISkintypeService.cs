using Services.Commons.DTOs.Category;
using BusinessObject.Entities;
using Services.Commons;
using Services.Commons.DTOs.SkinType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Commons.DTOs.Skintype;

namespace Services.Interfaces
{
    public interface ISkintypeService
    {

        Task<SkinType> GetSkinTypeById(Guid id);

        Task<ServiceResult> CreateSkinType(SkinTypeCreationDTO skintype);

        Task<ServiceResult> UpdateSkinType(Guid id, SkinType skintype);
        Task<ServiceResult<ICollection<SkintypeDetailDTO>>> GetAllSkintype();
    }
}
