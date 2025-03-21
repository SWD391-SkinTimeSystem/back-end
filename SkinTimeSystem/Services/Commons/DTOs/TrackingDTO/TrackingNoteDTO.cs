using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.TrackingDTO
{
    public class TrackingNoteDTO
    {
        public Guid TrackingId {  get; set; }
        public  string Note { get; set; }
    }
}
