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
            CreateMap<User, UserAddTest>()
                .ForMember(dest => dest.Name,opt =>opt.MapFrom(src =>src.FullName))
                .ReverseMap();
        }
    }
}
