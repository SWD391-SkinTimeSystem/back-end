using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SkinTime.DAL.Entities;
using SkinTime.Models;

namespace SkinTime.Helpers
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CustomerRegistration, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email)) // Custmer account username will be the email address.
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "Customer"));

            CreateMap<AccountRegistration, User>();

            CreateMap<AccountUpdateInformation, User>();

            CreateMap<User, AccountInformation>().ReverseMap();

            CreateMap<Feedback, TherapistFeedbackViewModel>()
                .ForMember(dest => dest.FeedbackId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.TherapistRating))
                .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => src.TherapistFeedback))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.BookingNavigation.CustomerId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.BookingNavigation.CustomerNavigation.FullName));

            CreateMap<Therapist, TherapistViewModel>()
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.UserNavigation.FullName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.UserNavigation.Avatar))
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.BIO))
                .ForMember(dest => dest.CertificationsUrl, opt => opt.MapFrom(src => src.CertificationNavigation.IsNullOrEmpty() ?
                default :src.CertificationNavigation.Select(x => x.FileUrl)))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.BookingNavigation.IsNullOrEmpty() ?
                default : src.BookingNavigation.Select(x => x.FeedbackNavigation)));

            CreateMap<Schedule,ScheduleViewModel>()
                .ForMember(dest => dest.ServiceStepId, opt => opt.MapFrom(src => src.ServiceDetailId))
                .ForMember(dest => dest.ServiceStepName, opt => opt.MapFrom(src => src.ServiceDetailNavigation.Name))
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceDetailNavigation.ServiceID))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.ReservedStartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.ReservedEndTime))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Step, opt => opt.MapFrom(src => src.ServiceDetailNavigation.Step));

            CreateMap<Booking, BookingViewModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerNavigation.FullName))
                .ForMember(dest => dest.TherapistName, opt => opt.MapFrom(src => src.TherapistNavigation.UserNavigation.FullName))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceNavigation.ServiceName));

            CreateMap<Booking, BookingDetailViewModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.TotalPayment))
                .ForMember(dest => dest.TherapistName, opt => opt.MapFrom(src => src.TotalPayment))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.TotalPayment))
                .ForMember(dest => dest.PaymentValue, opt => opt.MapFrom(src => src.TotalPayment))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.TransactionId != null ? "Success" : "Failed"))
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.ScheduleNavigation))
                .ForMember(dest => dest.VoucherPercentage, opt => opt
                    .MapFrom(src => src.VoucherNavigation == null ? 0 : src.VoucherNavigation.Discount));

            CreateMap<Question, QuestionModel>()
                .ForMember(dest => dest.No, opt => opt.MapFrom(src => src.OrderNo))
                .ForMember(dest => dest.IdQuestion, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.QuestionOptionsNavigation));

            CreateMap<QuestionOption, QuestionOptionModel>();

            CreateMap<SkinType, SkintypePercentage>()
            .ForMember(dest => dest.NameSkinType, opt => opt.MapFrom(src => src.Name));

            CreateMap<Service, ServiceRecommendationModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.NameService, opt => opt.MapFrom(src => src.ServiceName));

            CreateMap<(Dictionary<SkinType, double>, List<Service>), AnalysisModel>()
                .ConstructUsing(src => new AnalysisModel
                {
                    SkinTypes = src.Item1.Select(kvp => new SkintypePercentage
                    {
                        NameSkinType = kvp.Key.Name,
                        Percentage = kvp.Value
                    }).ToList(),
                    Services = src.Item2.Select(s => new ServiceRecommendationModel
                    {
                        Id = s.Id.ToString(),
                        NameService = s.ServiceName
                    }).ToList()
                });

            // Map từ Service -> ServiceModel
            CreateMap<Service, ServiceModel>()
                .ForMember(dest => dest.ServiceDetails, opt => opt.MapFrom(src => src.ServiceDetailNavigation))
                .ForMember(dest => dest.ServiceImages, opt => opt.MapFrom(src => src.ServiceImageNavigation))
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore());

            // Map từ ServiceDetail -> ServiceDetailModel
            CreateMap<ServiceDetail, ServiceDetailModel>();

            // Map từ ServiceImage -> ServiceImageModel
            CreateMap<ServiceImage, ServiceImageModel>();

            // Map từ (Booking?, Feedback?, User?) -> FeedBackServiceModel
            CreateMap<(Booking?, Feedback?, User?), FeedBackServiceModel>()
                .ForMember(dest => dest.CustommerName, opt => opt.MapFrom(src => src.Item3 != null ? src.Item3.FullName : "Unknown")) 
                .ForMember(dest => dest.Star, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.ServiceRating : (int?)null))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.CreatedTime :(DateTime ?)null))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.ServiceFeedback : null)); 

            // Map từ (Service, List<(Booking?, Feedback?, User?)>?) -> ServiceModel
            CreateMap<(Service, List<(Booking?, Feedback?, User?)>?), ServiceModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item1.Id))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Item1.ServiceName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Item1.Description))
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.Item1.Thumbnail))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Item1.Price))
                .ForMember(dest => dest.ServiceDetails, opt => opt.MapFrom(src => src.Item1.ServiceDetailNavigation))
                .ForMember(dest => dest.ServiceImages, opt => opt.MapFrom(src => src.Item1.ServiceImageNavigation))
                .ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.Item2 ?? new List<(Booking?, Feedback?, User?)>())); // Nếu null, chuyển thành list rỗng
        }

    }
}
