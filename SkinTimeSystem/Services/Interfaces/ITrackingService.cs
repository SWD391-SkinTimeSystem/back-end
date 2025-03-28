
using BusinessObject.Entities;
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
        Task<ServiceResult> CreateTracking(CreationalTrackingDTO creationalTrackingDTO);
        Task<ServiceResult<string>> NoteTracking(TrackingNoteDTO trackingNoteDTO);
        Task<ServiceResult<string>> CheckoutTracking(Guid scheduleId);

        Task<ServiceResult> CheckScheduleWithTrackId(Guid scheduleID);
    }
}
