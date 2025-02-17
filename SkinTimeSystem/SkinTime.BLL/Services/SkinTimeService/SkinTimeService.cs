using Entities;
using Microsoft.EntityFrameworkCore;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
        public async Task<(Service?, List<(Booking?, Feedback?, User?)>?)> GetService(Guid idService)
        {
            var service = await _unitOfWork.Repository<Service>()
                .GetByConditionAsync(s => s.Id == idService,
                    query => query.Include(s => s.ServiceDetailNavigation)
                                  .Include(s => s.ServiceImageNavigation));

            var bookings = await _unitOfWork.Repository<Booking>()
                .ListAsync(
                    filter: b => b.ServiceId == idService,
                    orderBy: null,
                    includeProperties: query => query
                        .Include(b => b.CustomerNavigation)
                        .Include(b => b.FeedbackNavigation!)
                );

            var result = bookings
                .Where(b => b.FeedbackNavigation != null) 
                .Select(b => (b, b.FeedbackNavigation!, b.CustomerNavigation))
                .ToList();

            return (service, result.Any() ? result : null);
        }

        public async Task<(Service?, List<ServiceDetail>)> GetTreatmentPlan(Guid idService)
        {
            var service = _unitOfWork.Repository<Service>().GetEntityByIdAsync(idService);

            var serviceDetails = await _unitOfWork.Repository<ServiceDetail>().ListAsync(
                filter: b => b.ServiceID == idService,
                orderBy: null,
                includeProperties: null
            );

            return (service, serviceDetails);
        }

    }


}

