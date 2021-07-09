using AutoMapper;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastucture.MapperConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostDTO>().ForMember(d => d.PostId, m => m.MapFrom(s => s.Id)).ReverseMap();
            CreateMap<Security, SecurityDTO>().ForMember(d => d.SecurityId, m => m.MapFrom(s => s.Id)).ReverseMap(); 
        }
    }
}
