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
        }
    }
}
