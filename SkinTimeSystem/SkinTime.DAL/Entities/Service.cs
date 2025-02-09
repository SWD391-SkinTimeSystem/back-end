using Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class Service : BaseEntity
    {
        [Column("service_name", TypeName = "NVARCHAR")]
        [MaxLength(50)]
        public required string ServiceName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Thumbnail { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Status { get; set; }

        [ForeignKey(nameof(ServiceCategory))]
        public Guid ServiceCategoryID { get; set; }

        // Virtual properties for relationship navigation
        public ServiceCategory? ServiceCategory { get; set; }
        public virtual ICollection<ServiceRecommendation> ServiceRecommendationNavigation { get; set; } = new Collection<ServiceRecommendation>();
        public virtual ICollection<ServiceDetail> ServiceDetailNavigation { get; set; } = new Collection<ServiceDetail>();
        public virtual ICollection<ServiceImage> ServiceImageNavigation { get; set; } = new Collection<ServiceImage>();
    }
}
