
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class UserChoice : BaseEntity
    {
        [ForeignKey("QuestionOption")]
        public Guid QuestionOptionID{ get; set; }
        public virtual QuestionOption? QuestionOptions { get; set; }
        [ForeignKey("User")]
        public Guid UserID { get; set; }
        public virtual User? Users{ get; set; }
    }
}
