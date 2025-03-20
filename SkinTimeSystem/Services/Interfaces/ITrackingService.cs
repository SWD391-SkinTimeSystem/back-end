
using Services.Commons;
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
        Task<ServiceResult<string>> CreateTracking(CreationalTrackingDTO creationalTrackingDTO);
        Task<ServiceResult<string>> NoteTracking(Guid trackingId, string note);
        Task<ServiceResult<string>> CheckoutTracking(Guid trackingId);
    }
}
