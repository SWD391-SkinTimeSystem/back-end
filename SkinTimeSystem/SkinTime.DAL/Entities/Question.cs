using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class Question : BaseEntity
    {
        [Column("order_no")]
        public int? OrderNo { get; set; }

        [Column("content", TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public required string Content { get; set; }

        // virtual navigation properties.
        public virtual ICollection<QuestionOption> QuestionOptionsNavigation { get;set; } = new List<QuestionOption>();
    }
}
