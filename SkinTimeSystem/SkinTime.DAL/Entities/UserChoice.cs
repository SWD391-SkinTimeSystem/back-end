
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
        [ForeignKey("Question")]
        public Guid QuestionID { get; set; }
        public virtual Question? Questions { get; set; }
        [ForeignKey("User")]
        public Guid UserID { get; set; }
        public virtual User? Users{ get; set; }
    }
}
