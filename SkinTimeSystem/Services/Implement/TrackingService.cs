using AutoMapper;
using BusinessObject.Entities;
using Repositories.Data;
using Repositories.UnitOfWork;
using Services.Commons;
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



        public async Task<ServiceResult<string>> CreateTracking(CreationalTrackingDTO creationalTrackingDTO)
        {
            var scheduleId = creationalTrackingDTO.ScheduleId;
            var otpInput = creationalTrackingDTO.OtpInput;

            if (scheduleId == null)
            {
                return ServiceResult<string>.Failed(ServiceError.ValidationFailed("Schedule ID is required"));
            }

            var isExistSchedule = await IsValidScheduleAsync(scheduleId);

            if (!isExistSchedule)
            {
                return ServiceResult<string>.Failed(ServiceError.ValidationFailed("Schedule doesn't exist"));
            }



            // Giải mã CheckInCode từ ScheduleId
            string expectedCheckInCode = GenerateCheckInCode(scheduleId);
            Console.WriteLine(expectedCheckInCode);

            // So sánh với OTP nhập vào
            if (expectedCheckInCode != otpInput)
            {
                return ServiceResult<string>.Failed(ServiceError.ValidationFailed("Invalid OTP"));
            }

            // Nếu OTP đúng, tiếp tục xử lý tracking
            var tracking = _mapper.Map<Tracking>(creationalTrackingDTO);
            var result =  await _unitOfWork.Trackings.CreateTracking(tracking);
            return ServiceResult<string>.Success("Tracking created successfully");

        }

        private async Task<bool> IsValidScheduleAsync(Guid scheduleId)
        {
            bool schedule = await _unitOfWork.Schedules.GetScheduleById(scheduleId);

            if (schedule)
            {
                return true; //  tìm thấy lịch trình

            }

            return false; //không tìm thấy
        }

        private string GenerateCheckInCode(Guid scheduleId)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(scheduleId.ToString())).Substring(0, 6);
        }

        public async Task<ServiceResult<string>> NoteTracking(Guid trackingId, string note)
        {
            await _unitOfWork.Trackings.NoteTracking(trackingId, note);
            return ServiceResult<string>.Success("Tracking note successfully");

        }





        public async Task<ServiceResult<string>> CheckoutTracking(Guid trackingId) { 
            await _unitOfWork.Trackings.CheckoutTracking(trackingId);
            return ServiceResult<string>.Success("Tracking checkout successfully");

        }
    }

}
