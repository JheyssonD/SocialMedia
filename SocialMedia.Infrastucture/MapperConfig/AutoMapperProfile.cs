using AutoMapper;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastucture.MapperConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostDTO>().ForMember(d => d.PostId, m => m.MapFrom(s => s.Id));
            CreateMap<PostDTO, Post>().ForMember(d => d.Id, m => m.MapFrom(s => s.PostId));
        }
    }
}
