using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface ITrackingRepository : IGenericRepository<Tracking>
    {
        Task<Tracking> CreateTracking(Tracking tracking);
        Task<bool> NoteTracking(Guid trackingId, string note);
        Task<bool> CheckoutTracking(Guid scheduleId);

        Task<Tracking> GetTrackingWithSchedulId(Guid scheduleId);


    }
}
