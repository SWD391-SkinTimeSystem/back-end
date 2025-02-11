using Microsoft.EntityFrameworkCore;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.SkinTimeService
{
    public class SkinTimeService : ISkinTimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SkinTimeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<(Service?, List<(Booking?,  Feedback?, User?)>?)> GetService(Guid idService)
        {
            var service = await _unitOfWork.Repository<Service>()
                              .GetByConditionAsync(s => s.Id == idService,
                                  query => query.Include(s => s.ServiceDetailNavigation)
                                                .Include(s => s.ServiceImageNavigation));

            if (service == null)
                return (null, null);

            var bookings = await _unitOfWork.Repository<Booking>()
                .ListAsync(
                    filter: b => b.ServiceId == idService,
                    orderBy: null,
                    includeProperties: query => query
                        .Include(b => b.CustomerNavigation)
                        .Include(b => b.FeedbackNavigation!)
                );
            var result = bookings.Any()
                  ? bookings.Select(b => (b, b.FeedbackNavigation,b.CustomerNavigation)).ToList()
                  : null;

            return (service, result);
        }

    }


}

