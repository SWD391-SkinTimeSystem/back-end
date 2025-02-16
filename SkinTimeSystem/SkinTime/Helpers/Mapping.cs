using AutoMapper;
using Entities;
using SkinTime.DAL.Entities;
using SkinTime.Models;

namespace SkinTime.Helpers
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserAdd>().ReverseMap();
            CreateMap<User, UserAddWithRole>().ReverseMap();  

            CreateMap<CustomerRegistration, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) // Custmer account username will be the email address.
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "Customer"));

            CreateMap<AccountRegistration, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));

            CreateMap<AccountUpdateInformation, User>();

            CreateMap<User, AccountInformation>()
                .ReverseMap();

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
