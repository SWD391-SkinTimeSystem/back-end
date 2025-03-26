using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.SkinType
{
    public class SkinTypeCreationDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
