using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories;

namespace Services.Commons.DTOs.Notification
{
    public class NotificationWithCountDTO
    {
       public int NumberUnread {  get; set; }
        public PaginationResult<NotificationAll> Notifications { get; set; } = new();
    }
    public class NotificationAll
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid ToUserId { get; set; }
        public bool IsRead { get; set; }
        public Guid? AboutId { get; set; }
        public DateTime CreatedTime { get; set; }
    }

}
