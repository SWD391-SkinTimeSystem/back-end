using AutoMapper;
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

            CreateMap<Event, EventInformation>()
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Image, opt => opt.MapFrom(src => src.Thubmnail))
                .ForMember(dst => dst.TotalTickets, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(dst => dst.AvailableTickets, opt => opt.MapFrom(src => src.Capacity - src.EventTickets.Count))
                .ForMember(dst => dst.EventStatus, opt => opt.MapFrom(src => src.Status));

            CreateMap<EventCreation, Event>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.EventName))
                .ForMember(dst => dst.Thubmnail, opt => opt.MapFrom(src => src.EventImage))
                .ForMember(dst => dst.TicketPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dst => dst.TimeStart, opt => opt.MapFrom(src => src.Date.ToDateTime(src.StartTime)))
                .ForMember(dst => dst.TimeEnd, opt => opt.MapFrom(src => src.Date.ToDateTime(src.EndTime)))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => "Available"));
        }
    }
}
