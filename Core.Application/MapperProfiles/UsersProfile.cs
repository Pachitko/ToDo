using Core.Application.Features.Commands.CreateUser;
using Core.Domain.Entities;
using AutoMapper;

namespace Core.Application.MapperProfiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<CreateUser.Command, AppUser>();
               //.ForPath(dest => dest.UserProfile.FirstName, o => o.MapFrom(src => src.FirstName))
               //.ForPath(dest => dest.UserProfile.LastName, o => o.MapFrom(src => src.LastName));
        }
    }
}