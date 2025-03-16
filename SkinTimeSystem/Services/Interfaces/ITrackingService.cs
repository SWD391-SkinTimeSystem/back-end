
using Services.Commons.DTOs.TrackingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITrackingService 
    {
        Task<bool> CreateTracking(CreationalTrackingDTO creationalTrackingDTO);
        Task<bool> NoteTracking(Guid trackingId, string note);
        Task<bool> CheckoutTracking(Guid trackingId);
    }
}
