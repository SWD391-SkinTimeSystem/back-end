using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class Message : BaseEntity
    {
        [Column("content", TypeName = "NVARCHAR")]
        [MaxLength(2048)]
        public string? Content { get; set; }
        [Column("is_read")]
        public bool IsRead { get; set; } = false;
        [Column("to_user_id")]
        public required Guid ToUserId { get; set; }
        [Column("reaction")]
        public string? Reaction { get; set; }

        [Column("user_id")]
        [ForeignKey(nameof(User))]
        public required Guid UserId { get; set; }
        public virtual User Users { get; set; } = null!;
    }
}
