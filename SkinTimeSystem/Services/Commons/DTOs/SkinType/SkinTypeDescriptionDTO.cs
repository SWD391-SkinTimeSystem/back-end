using Services.Commons.DTOs.Skintype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.SkinType
{
    public  class SkinTypeDescriptionDTO : SkintypeDetailDTO
    {

        public string? Description { get; set; } = null;
    }
}
