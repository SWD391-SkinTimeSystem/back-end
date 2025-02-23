using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities {
    public class ServiceDetail: BaseEntity
    {
        [Column("name", TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string Name{  get; set; } = string.Empty;

        [Column("description", TypeName = "NVARCHAR")]
        [MaxLength(1024)]
        public string Description { get; set; } = string.Empty;

        [Column("step")]
        public int Step {  get; set; }

        [Column("duration")]
        public int Duration { get; set; }

        [Column("price", TypeName = "DECIMAL")]
        [Precision(16,2)]
        public decimal UnitPrice { get; set; }

        [Column("day_to_next_step")]
        public int DateToNextStep {  get; set; }

        [Column("is_deleted")]
        public bool IsDetele {  get; set; }

        [Column("service_id")]
        [ForeignKey(nameof(Service))]
        public Guid ServiceID { get; set; }

        // Virtual properties represent entity relationship.
        public virtual Service ServiceNavigation { get; set; } = null!;
        public virtual ICollection<Schedule> ScheduleNavigation { get; set; } = new Collection<Schedule>();
    }
}