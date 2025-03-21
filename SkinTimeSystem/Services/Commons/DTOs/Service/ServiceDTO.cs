using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Service
{
    public class ServiceDTO
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Thumbnail { get; set; }
        public decimal Price { get; set; }

        public List<ServiceDetailDTO> ServiceDetails { get; set; }
        public List<ServiceImageDTO> ServiceImages { get; set; }

    }
    
    
    
}
