using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class QuestionOption : BaseEntity
    {
        [Column("content", TypeName = "NVARCHAR(250)")]
        public required string Content {  get; set; }

        [Column("question_id")]
        [ForeignKey(nameof(SkinType))]
        public required Guid QuestionID { get; set; }

        public virtual ICollection<QuestionOptionSkintype> QuestionOptionSkintypes { get; set; } = new List<QuestionOptionSkintype>();

    }
}
