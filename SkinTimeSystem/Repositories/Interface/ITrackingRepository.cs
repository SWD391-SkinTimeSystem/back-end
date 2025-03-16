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
        Task<bool> CreateTracking(Tracking tracking);
        Task<bool> NoteTracking(Guid trackingId, string note);
        Task<bool> CheckoutTracking(Guid trackingId);
    }
}
