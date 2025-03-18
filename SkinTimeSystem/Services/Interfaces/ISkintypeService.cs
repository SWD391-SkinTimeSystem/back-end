using Services.Commons.DTOs.Category;
using Services.Commons;
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
        Task<ServiceResult<ICollection<SkintypeDetailDTO>>> GetAllSkintype();
    }
}
