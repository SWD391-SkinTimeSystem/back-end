using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities {
    public class ServiceDetail: BaseEntity
    {
        public  string Name{  get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Step {  get; set; }
        public int Duration { get; set; }
        public int DateToNextStep {  get; set; }
        public bool IsDetele {  get; set; }

        [ForeignKey("Service")]
        public Guid ServiceID { get; set; }

        // Virtual properties represent entity relationship.
        public virtual Service Service { get; set; } = null!;
        public virtual ICollection<Schedule> ScheduleNavigation { get; set; } = new Collection<Schedule>();
    }
}