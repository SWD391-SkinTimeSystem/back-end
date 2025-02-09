using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class ServiceImage : BaseEntity
    {
        public string? ImageUrl { get; set; }
        [ForeignKey("Service")]
        public Guid? ServiceId {  get; set; }
        public virtual Service? Service { get; set; }// khoa ngoai cua moi quan he 1 service co nhieu anh 
    }
}
