using AutoMapper;
using BusinessObject.Entities;
using Repositories;
using Repositories.Data;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.Ticket;
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
        public TrackingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        public async Task<ServiceResult> CreateTracking(CreationalTrackingDTO creationalTrackingDTO)
        {
            var scheduleId = creationalTrackingDTO.ScheduleId;
            var otpInput = creationalTrackingDTO.OtpInput;


            var isExistSchedule = await IsValidScheduleAsync(scheduleId);

            if (!isExistSchedule)
            {
                return ServiceResult<string>.Failed(ServiceError.ValidationFailed("Schedule doesn't exist"));
            }

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
            var time = result.CheckinTime;         


            if (time == null)
            {
                return ServiceResult<TimeOnly>.Failed(ServiceError.ValidationFailed("Don't checkin"));
            }

            string formattedTime = time.Value.ToString("HH:mm:ss");
            return ServiceResult<string>.Success(formattedTime);


            

        }

        private async Task<bool> IsValidScheduleAsync(Guid scheduleId)
        {
            bool schedule = await _unitOfWork.Schedules.CheckScheduleById(scheduleId);

            if (schedule)
            {
                return true; //  tìm thấy lịch trình

            }

            return false; //không tìm thấy
        }

        private string GenerateCheckInCode(Guid scheduleId)
        {
            return Math.Abs(BitConverter.ToInt32(scheduleId.ToByteArray(), 0))
                   .ToString().PadLeft(6, '0').Substring(0, 6);
        }


        public async Task<ServiceResult<string>> NoteTracking(TrackingNoteDTO trackingNoteDTO )
        {
            await _unitOfWork.Trackings.NoteTracking(trackingNoteDTO.TrackingId,trackingNoteDTO.Note);
            return ServiceResult<string>.Success("Tracking note successfully");

        }


        public async Task<ServiceResult<string>> CheckoutTracking(Guid scheduleId) { 
            await _unitOfWork.Trackings.CheckoutTracking(scheduleId);
            return ServiceResult<string>.Success("Tracking checkout successfully");

        }



        public async Task<ServiceResult> CheckScheduleWithTrackId(Guid scheduleID)
        {
            var schedule = await _unitOfWork.Schedules.GetScheduleById(scheduleID);

            var tracking = await _unitOfWork.Trackings.GetTrackingWithSchedulId(scheduleID);

            var isCheckin = false;
            string formattedTime = "";
            if (tracking != null)
            {
                isCheckin = true;
                formattedTime = tracking.CheckinTime.Value.ToString("HH:mm:ss");

            }
            
            return ServiceResult<ScheduleTrackingDTO>.Success(new ScheduleTrackingDTO
            {
                ScheduleId = scheduleID,
                CheckinTime = formattedTime,
                isCheckin = isCheckin
            });


        }
    }

}
