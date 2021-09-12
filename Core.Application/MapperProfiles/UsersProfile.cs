using Core.Application.Features.Commands.CreateFullUser;
using Core.Application.Features.Commands.CreateUser;
using Core.Domain.Entities;
using AutoMapper;

namespace Core.Application.MapperProfiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<CreateUser.Command, AppUser>()
               .ForPath(dest => dest.UserProfile.FirstName, o => o.MapFrom(src => src.FirstName))
               .ForPath(dest => dest.UserProfile.LastName, o => o.MapFrom(src => src.LastName));

            CreateMap<CreateFullUser.Command, AppUser>()
              .ForPath(dest => dest.UserProfile.FirstName, o => o.MapFrom(src => src.FirstName))
              .ForPath(dest => dest.UserProfile.MiddleName, o => o.MapFrom(src => src.MiddleName))
              .ForPath(dest => dest.UserProfile.LastName, o => o.MapFrom(src => src.LastName))
              .ForPath(dest => dest.UserProfile.DateOfBirth, o => o.MapFrom(src => src.DateOfBirth))
              .ForPath(dest => dest.UserProfile.DateOfDeath, o => o.MapFrom(src => src.DateOfDeath));
        }
    }
}