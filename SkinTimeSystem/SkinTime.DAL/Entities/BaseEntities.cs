using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class BaseEntity
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        // The time created the object will be used as the time the new record is added into the database.

        [Column("last_update")]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}
