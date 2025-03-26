using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.Service
{
    public class ServiceCreateAdvandDTO
    {
        public Guid IdService { get; set; }
        public required IFormFile Thumbnail { get; set; }
        public required ICollection<IFormFile> ServiceImages { get; set; }
    }
}
