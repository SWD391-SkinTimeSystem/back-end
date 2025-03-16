using AutoMapper;
using BusinessObject.Entities;
using Repositories.Data;
using Repositories.UnitOfWork;
using Services.Commons.DTOs.TrackingDTO;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class TrackingService : ITrackingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public TrackingService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;

        }

      

        public async Task<bool> CreateTracking(CreationalTrackingDTO creationalTrackingDTO)
        {
            var tracking = _mapper.Map<Tracking>(creationalTrackingDTO);          
            return await _unitOfWork.Trackings.CreateTracking(tracking);

        }

        public async Task<bool> NoteTracking(Guid trackingId, string note) => await _unitOfWork.Trackings.NoteTracking(trackingId, note);

        



        public async Task<bool> CheckoutTracking(Guid trackingId) => await _unitOfWork.Trackings.CheckoutTracking(trackingId);
    }

}
