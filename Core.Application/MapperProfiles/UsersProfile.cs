using Core.Application.Features.Commands.RegisterUser;
using Core.Application.Features.Commands.JwtRegister;
using Core.Domain.Entities;
using AutoMapper;

namespace Core.Application.MapperProfiles
{
    public class UsersProfile : Profile
    {
        public override string ProfileName => "User";

        public UsersProfile()
        {
            CreateMap<RegisterUser.Command, AppUser>()
                .ForPath(u => u.UserProfile.FirstName, o => o.MapFrom(c => c.FirstName))
                .ForPath(u => u.UserProfile.LastName, o => o.MapFrom(c => c.LastName))
                .ReverseMap();

            CreateMap<CreateUser.Command, AppUser>()
                .ForPath(u => u.UserProfile.FirstName, o => o.MapFrom(c => c.FirstName))
                .ForPath(u => u.UserProfile.LastName, o => o.MapFrom(c => c.LastName))
                .ReverseMap();
        }
    }
}