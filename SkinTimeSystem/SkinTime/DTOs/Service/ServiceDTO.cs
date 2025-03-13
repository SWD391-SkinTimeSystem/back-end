using BusinessObject.Entities;
using BusinessObject.Enum;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkinTime.DTOs.Service
{
    public class ServiceDTO
    {
        public required string ServiceName { get; set; }
        public required string Description { get; set; }
        public required string Thumbnail { get; set; } 
        public required int Duration { get; set; }// => tự tính dự vào duẩtion của từng step 
        public required decimal Price { get; set; }
        public required string Status { get; set; }// sẽ là string và sao đó sẽ map lại 

        public Guid ServiceCategoryID { get; set; }

        public required ICollection<Guid> SkintypeIds { get; set; }
        public required ICollection<ServiceDetailsDTO> ServiceDetails { get; set; }
        public required ICollection<IFormFile> ServiceImages { get; set; } 
    }
}
