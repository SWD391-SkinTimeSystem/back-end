using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class QuestionOptionSkintype : BaseEntity
    {
        [Column("skin_type_id")]
        [ForeignKey(nameof(SkinType))]
        public required Guid SkinTypeID { get; set; }

        public virtual SkinType SkinTypeNavigation { get; set; } = null!;

        [Column("question_option_id")]
        [ForeignKey(nameof(QuestionOption))]
        public required Guid QuestionOptionId { get; set; }

        public virtual QuestionOption QuestionOptions { get; set; } = null!;
    }
}
