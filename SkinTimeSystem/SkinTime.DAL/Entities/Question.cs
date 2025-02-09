using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class Question : BaseEntity
    {
        public int? OrderNo { get; set; }
        public string? Content { get; set; }
        public virtual ICollection<QuestionOption> QuestionOptions{ get;set; }
    }
}
