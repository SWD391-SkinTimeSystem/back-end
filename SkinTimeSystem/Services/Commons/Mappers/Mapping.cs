using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enum;
using BusinessObject.EventEnums;
using Services.Commons.Analysis;
using Services.Commons.DTOs.Analysis;
using Services.Commons.DTOs.Booking;
using Services.Commons.DTOs.Event;
using Services.Commons.DTOs.Feedback;
using Services.Commons.DTOs.Question;
using Services.Commons.DTOs.Schedule;
using Services.Commons.DTOs.Service;
using Services.Commons.DTOs.Therapist;
using Services.Commons.DTOs.Ticket;
using Services.Commons.DTOs.TrackingDTO;
using Services.Commons.DTOs.User;
using System.Net.NetworkInformation;
using System.Text;

namespace SkinTime.Helpers
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CustomerRegistration, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email)) 
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "Customer"));

            CreateMap<AccountRegistration, User>();

            CreateMap<AccountUpdateInformation, User>();

            CreateMap<User, AccountInformation>().ReverseMap();

            CreateMap<FeedbackCreationDTO, Feedback>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<Feedback, BookingFeedbackDTO>()
                .ForMember(dest => dest.FeedbackId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.BookingNavigation.CustomerId));

            CreateMap<Feedback, TherapistFeedbackDTO>()
                .ForMember(dest => dest.FeedbackId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.TherapistRating))
                .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => src.TherapistFeedback))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.BookingNavigation.CustomerId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.BookingNavigation.CustomerNavigation.FullName));

            //CreateMap<Feedback, ServiceFeedbackDTO>()
            //    .ForMember(dest => dest.FeedbackId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.BookingNavigation.CustomerId))
            //    .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.BookingNavigation.CustomerNavigation.Username))
            //    .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => (src.TherapistRating + src.ServiceRating) / 2.0f)) // Trung bình rating
            //    .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => $"{src.TherapistFeedback} | {src.ServiceFeedback}".Trim()))
            //    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BookingNavigation.ReservedTime)))
            //    .ReverseMap();

            CreateMap<Therapist, TherapistDTO>()
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.UserNavigation.FullName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.UserNavigation.Avatar))
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.BIO))
                .ForMember(dest => dest.CertificationsUrl, opt => opt.MapFrom(src => src.CertificationNavigation.Count == 0 ?
                default : src.CertificationNavigation.Select(x => x.FileUrl)))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.BookingNavigation.Count == 0 ?
                default : src.BookingNavigation.Select(x => x.FeedbackNavigation).Where(x => x != null)));

            CreateMap<Schedule, ScheduleDTO>()
                .ForMember(dest => dest.ServiceStepId, opt => opt.MapFrom(src => src.ServiceDetailId))
                .ForMember(dest => dest.ServiceStepName, opt => opt.MapFrom(src => src.ServiceDetailNavigation.Name))
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceDetailNavigation.ServiceID))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.ReservedStartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.ReservedEndTime))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Step, opt => opt.MapFrom(src => src.ServiceDetailNavigation.Step));

            //CreateMap<Booking, BookingDTO>()
            //    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerNavigation.FullName))
            //    .ForMember(dest => dest.TherapistName, opt => opt.MapFrom(src => src.TherapistNavigation.UserNavigation.FullName))
            //    .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceNavigation.ServiceName));

            //CreateMap<Booking, BookingDetailDTO>()
            //    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.TotalPayment))
            //    .ForMember(dest => dest.TherapistName, opt => opt.MapFrom(src => src.TotalPayment))
            //    .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.TotalPayment))
            //    .ForMember(dest => dest.PaymentValue, opt => opt.MapFrom(src => src.TotalPayment))
            //    .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.TransactionId != null ? "Success" : "Failed"))
            //    .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.ScheduleNavigation))
            //    .ForMember(dest => dest.VoucherPercentage, opt => opt
            //        .MapFrom(src => src.VoucherNavigation == null ? 0 : src.VoucherNavigation.Discount));

            CreateMap<Question, QuestionDTO>()
                .ForMember(dest => dest.No, opt => opt.MapFrom(src => src.OrderNo))
                .ForMember(dest => dest.IdQuestion, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.QuestionOptionsNavigation));

            CreateMap<QuestionOption, QuestionOptionDTO>();

            CreateMap<QuestionCreationDTO, Question>()
                .ForMember(dest => dest.QuestionOptionsNavigation, opt => opt.MapFrom(src => src.Choices));

            //CreateMap<QuestionChoiceCreationDTO, QuestionOption>()
            //    .ForMember(dest => dest.SkinTypeID, opt => opt.MapFrom(src => src.SkinType));

            CreateMap<EventTicket, TicketViewModel>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
                .ForMember(dest => dest.TicketId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Otp, opt => opt.MapFrom(src => src.TicketCode))
                .ForMember(dest => dest.QRCode, opt => opt.MapFrom(src => src.QRCode))
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.EventNavigation.Name))
                .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.PaidAmount))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            //CreateMap<TicketRegistrationDTO, TicketRegistrationCacheDTO>();

            //CreateMap<TicketRegistrationCacheDTO, EventTicket>()
            //    .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
            //    .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserId))
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(x => EventTicketStatus.Paid))
            //    .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => src.TotalAmount))
            //    .ForMember(dest => dest.TicketCode, opt => opt.MapFrom(src => src.Ticket_Otp))
            //    .ForMember(dest => dest.QRCode, opt => opt.MapFrom(src => src.Base64_QrCode));

            CreateMap<SkinType, SkintypePercentageDTO>()
            .ForMember(dest => dest.NameSkinType, opt => opt.MapFrom(src => src.Name));

            CreateMap<Service, ServiceRecommendationDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.NameService, opt => opt.MapFrom(src => src.ServiceName));

            CreateMap<(Dictionary<SkinType, double>, List<Service>), AnalysisDTO>()
                .ConstructUsing(src => new AnalysisDTO
                {
                    SkinTypes = src.Item1.Select(kvp => new SkintypePercentageDTO
                    {
                        NameSkinType = kvp.Key.Name,
                        Percentage = kvp.Value
                    }).ToList(),
                    Services = src.Item2.Select(s => new ServiceRecommendationDTO
                    {
                        Id = s.Id.ToString(),
                        NameService = s.ServiceName
                    }).ToList()
                });

            // Map từ Service -> ServiceDTO
            CreateMap<Service, ServiceDTO>()
                .ForMember(dest => dest.ServiceDetails, opt => opt.MapFrom(src => src.ServiceDetailNavigation))
                .ForMember(dest => dest.ServiceImages, opt => opt.MapFrom(src => src.ServiceImageNavigation))
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore()).ReverseMap();

            // Map từ ServiceDetail -> ServiceDetailDTO
            CreateMap<ServiceDetail, ServiceDetailDTO>().ReverseMap(); ;

            // Map từ ServiceImage -> ServiceImageDTO
            CreateMap<ServiceImage, ServiceImageDTO>().ReverseMap(); ;

            // Map từ (Booking?, Feedback?, User?) -> FeedBackServiceDTO
            //CreateMap<(Booking?, Feedback?, User?), ServiceFeedbackDTO>()
            //    .ForMember(dest => dest.CustommerName, opt => opt.MapFrom(src => src.Item3 != null ? src.Item3.FullName : "Unknown"))
            //    .ForMember(dest => dest.Star, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.ServiceRating : (int?)null))
            //    .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.CreatedTime : (DateTime?)null))
             //   .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.ServiceFeedback : null));

            // Map từ (Service, List<(Booking?, Feedback?, User?)>?) -> ServiceDTO
            CreateMap<(Service, List<(Booking?, Feedback?, User?)>?), ServiceDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item1.Id))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Item1.ServiceName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Item1.Description))
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.Item1.Thumbnail))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Item1.Price))
                .ForMember(dest => dest.ServiceDetails, opt => opt.MapFrom(src => src.Item1.ServiceDetailNavigation))
                .ForMember(dest => dest.ServiceImages, opt => opt.MapFrom(src => src.Item1.ServiceImageNavigation))
                .ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.Item2 ?? new List<(Booking?, Feedback?, User?)>())); // Nếu null, chuyển thành list rỗng
           

            CreateMap<Event, EventDTO>()
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Image, opt => opt.MapFrom(src => src.Thumbnail))
                .ForMember(dst => dst.TotalTickets, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(dst => dst.Date, opt => opt.MapFrom(src => src.EventDate))
                .ForMember(dst => dst.StartTime, opt => opt.MapFrom(src => src.TimeStart))
                .ForMember(dst => dst.EndTime, opt => opt.MapFrom(src => src.TimeEnd))
                .ForMember(dst => dst.AvailableTickets, opt => opt.MapFrom(src => src.Capacity - src.TicketNavigation.Count))
                .ForMember(dst => dst.EventStatus, opt => opt.MapFrom(src => src.Status));

            CreateMap<Event, AvailableEventDTO>()
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.StartDate, opt => opt.MapFrom(src => src.EventDate))
                .ForMember(dst => dst.StartTime, opt => opt.MapFrom(src => src.TimeStart))
                .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.Thumbnail))
                .ForMember(dst => dst.EventId, opt => opt.MapFrom(src => src.Id));


            CreateMap<EventCreationDTO, Event>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.EventName))
                .ForMember(dst => dst.EventDate, opt => opt.MapFrom(src => src.Date))
                .ForMember(dst => dst.Thumbnail, opt => opt.MapFrom(src => src.EventImage))
                .ForMember(dst => dst.TicketPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dst => dst.TimeStart, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dst => dst.TimeEnd, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => "ApprovePending"));



            CreateMap<Question, QuestionDTO>()
                .ForMember(dest => dest.No, opt => opt.MapFrom(src => src.OrderNo))
                .ForMember(dest => dest.IdQuestion, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.QuestionOptionsNavigation));
            CreateMap<QuestionOption, QuestionOptionDTO>();
            CreateMap<SkinType, SkintypePercentageDTO>()
            .ForMember(dest => dest.NameSkinType, opt => opt.MapFrom(src => src.Name));

            CreateMap<Service, ServiceRecommendationDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.NameService, opt => opt.MapFrom(src => src.ServiceName));

            CreateMap<(Dictionary<SkinType, double>, List<Service>), AnalysisDTO>()
                .ConstructUsing(src => new AnalysisDTO
                {
                    SkinTypes = src.Item1.Select(kvp => new SkintypePercentageDTO
                    {
                        NameSkinType = kvp.Key.Name,
                        Percentage = kvp.Value
                    }).ToList(),
                    Services = src.Item2.Select(s => new ServiceRecommendationDTO
                    {
                        Id = s.Id.ToString(),
                        NameService = s.ServiceName
                    }).ToList()
                });
            // Map từ Service -> ServiceDTO
            CreateMap<Service, ServiceDTO>()
                .ForMember(dest => dest.ServiceDetails, opt => opt.MapFrom(src => src.ServiceDetailNavigation))
                .ForMember(dest => dest.ServiceImages, opt => opt.MapFrom(src => src.ServiceImageNavigation))
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore());

            // Map từ ServiceDetail -> ServiceDetailDTO
            CreateMap<ServiceDetail, ServiceDetailDTO>();

            // Map từ ServiceImage -> ServiceImageDTO
            CreateMap<ServiceImage, ServiceImageDTO>();

            // Map từ (Booking?, Feedback?, User?) -> FeedBackServiceDTO
            //CreateMap<(Booking?, Feedback?, User?), ServiceFeedbackDTO>()
            //    .ForMember(dest => dest.CustommerName, opt => opt.MapFrom(src => src.Item3 != null ? src.Item3.FullName : "Unknown"))
            //    .ForMember(dest => dest.Star, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.ServiceRating : (int?)null))
            //    .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.CreatedTime : (DateTime?)null))
            //    .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.ServiceFeedback : null));

            // Map từ (Service, List<(Booking?, Feedback?, User?)>?) -> ServiceDTO
            CreateMap<(Service, List<(Booking?, Feedback?, User?)>?), ServiceDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item1.Id))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Item1.ServiceName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Item1.Description))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Item1.Duration))
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.Item1.Thumbnail))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Item1.Price))
                .ForMember(dest => dest.ServiceDetails, opt => opt.MapFrom(src => src.Item1.ServiceDetailNavigation))
                .ForMember(dest => dest.ServiceImages, opt => opt.MapFrom(src => src.Item1.ServiceImageNavigation))
                .ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.Item2 ?? new List<(Booking?, Feedback?, User?)>())); // Nếu null, chuyển thành list rỗng

            CreateMap<Service, TreatmentPlanDTO>()
            .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.ServiceDetailNavigation));

            CreateMap<ServiceDetail, Step>()
                .ForMember(dest => dest.ServiceDetailId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ServiceDetailName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Step))
                .ForMember(dest => dest.DurationInMinutes, opt => opt.MapFrom(src => src.Duration));
            CreateMap<Booking, BokingServiceStatusDTO>()
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceNavigation.ServiceName))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.ReservedTime))
                .ForMember(dest => dest.TimeStart, opt => opt.MapFrom(src =>
                      src.ScheduleNavigation
                      .Where(s => s.ServiceDetailNavigation.Step == 1)
                      .Select(s => s.ReservedStartTime)
                      .FirstOrDefault()
                       ))
                .ForMember(dest => dest.IsTretmentPlan, opt => opt.MapFrom(src =>
               src.ServiceNavigation.ServiceDetailNavigation != null && src.ServiceNavigation.ServiceDetailNavigation.Any()
                            ))
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.ServiceNavigation.Thumbnail))
                .ForMember(dest => dest.TherapistName, opt => opt.MapFrom(src =>
                 src.TherapistNavigation != null && src.TherapistNavigation.UserNavigation != null
                 ? src.TherapistNavigation.UserNavigation.FullName
                  : "Not yet"))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ServiceNavigation.Description))
                .ReverseMap();

            CreateMap<Schedule, BookingStepDetailsDTO>()
                .ForMember(dest => dest.ScheduleID, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.CheckInCode, opt => opt.MapFrom(src =>
                     Convert.ToBase64String(Encoding.UTF8.GetBytes(src.Id.ToString())).Substring(0, 6)
                 ));

            CreateMap<Booking, BookingDetailDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.TherapistName, opt => opt.MapFrom(src =>
                    src.TherapistNavigation != null && src.TherapistNavigation.UserNavigation != null
                        ? src.TherapistNavigation.UserNavigation.FullName
                        : "Not yet"
                ))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceNavigation.ServiceName))
                .ForMember(dest => dest.TotalStep, opt => opt.MapFrom(src =>
                    src.ServiceNavigation.ServiceDetailNavigation != null
                        ? src.ServiceNavigation.ServiceDetailNavigation.Count
                        : 0
                ))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ServiceNavigation.Description))
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.ServiceNavigation.Thumbnail))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.ScheduleNavigation)) // Chỉ rõ cách map
                .ReverseMap();


            //CreateMap<Schedule, BookingStepDetails>()
            //    .ForMember(dest => dest.ServiceDetailsName, opt => opt.MapFrom(src => src.ServiceDetailNavigation.Name))
            //    .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.ReservedStartTime))
            //    .ForMember(dest => dest.StartEnd, opt => opt.MapFrom(src => src.ReservedEndTime))
            //    .ForMember(dest => dest.ReservedDate, opt => opt.MapFrom(src => src.Date.ToDateTime(TimeOnly.MinValue)))
            //    .ReverseMap();

            //CreateMap<Feedback, ServiceFeedbackDTO>()
            //.ForMember(dest => dest.FeedbackId, opt => opt.MapFrom(src => src.Id))
            //.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.BookingNavigation.CustomerId))
            //.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.BookingNavigation.CustomerNavigation.Username))
            //.ForMember(dest => dest.Rating, opt => opt.MapFrom(src => (src.TherapistRating + src.ServiceRating) / 2.0f)) // Trung bình rating
            //.ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => $"{src.TherapistFeedback} | {src.ServiceFeedback}".Trim()))
            //.ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BookingNavigation.ReservedTime))
            //).ReverseMap();



            //CreateMap<Service, PopularServicesDTO>()
            //    .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(x => x.Id))
            //    .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(x => x.ServiceName))
            //    .ForMember(dest => dest.BookingCount, opt => opt.MapFrom(x => x.BookingNavigation.Count()))
            //    .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(x => x.BookingNavigation.Sum(x => x.TotalPayment)));

            CreateMap<Booking, BookingServiceWithIdDTO>()
                .ForMember(dest => dest.ServiceDate, opt => opt.MapFrom(src => src.ReservedTime))
                .ReverseMap()
    .ForMember(dest => dest.ReservedTime, opt =>
        opt.MapFrom(src => src.ServiceDate.ToDateTime(TimeOnly.MinValue)));

            //    CreateMap<ServiceDTO, Service>()
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ServiceStatus>(src.Status)))
            //.ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.ServiceDetails.Sum(d => d.Duration))) // Tự tính Duration
            //.ForMember(dest => dest.ServiceDetailNavigation, opt => opt.MapFrom(src => src.ServiceDetails));

            CreateMap<ServiceDetailsDTO, ServiceDetail>()
               .ForMember(dest => dest.ServiceID, opt => opt.Ignore());
            CreateMap<CreationalTrackingDTO, Tracking>().ReverseMap();

                
        }

        

    }

}


