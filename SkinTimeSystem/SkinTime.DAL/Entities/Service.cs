using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SkinTime.DAL.Enum;
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
        public required string ServiceName { get; set; }

        [Column("service_description", TypeName = "NVARCHAR")]
        [MaxLength(1024)]
        public required string Description { get; set; }

        [Column("thumbnail_url", TypeName = "VARCHAR")]
        [MaxLength(255)]
        public required string Thumbnail { get; set; } = string.Empty;

        [Column("price", TypeName = "DECIMAL")]
        [Precision(16,2)]
        public required decimal Price { get; set; }

        [Column("status")]
        public ServiceStatus Status { get; set; } = ServiceStatus.Available;

        [Column("service_category_id")]
        [ForeignKey(nameof(ServiceCategory))]
        public Guid ServiceCategoryID { get; set; }

        // Virtual properties for relationship navigation
        public virtual ServiceCategory? ServiceCategory { get; set; }
        public virtual ICollection<ServiceRecommendation> ServiceRecommendationNavigation { get; set; } = new Collection<ServiceRecommendation>();
        public virtual ICollection<ServiceDetail> ServiceDetailNavigation { get; set; } = new Collection<ServiceDetail>();
        public virtual ICollection<ServiceImage> ServiceImageNavigation { get; set; } = new Collection<ServiceImage>();
        public virtual ICollection<Booking> BookingNavigation { get; set; } = new Collection<Booking>();
    }
}
