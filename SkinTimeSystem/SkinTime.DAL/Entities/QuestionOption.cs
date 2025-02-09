using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class QuestionOption : BaseEntity
    {
        public string? Content {  get; set; }
        public bool? IsDelete { get; set; }
        [ForeignKey("SkinType")]
        public Guid SkinTypeID { get; set; }
        public virtual SkinType? SkinType { get; set; }
        [ForeignKey("Question")]
        public Guid QuestionID { get; set; }
        public virtual Question? Questions { get; set; }
    }
}
