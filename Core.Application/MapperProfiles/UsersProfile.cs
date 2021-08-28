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
                //.IgnoreAllUnmapped()
                .ForMember(u => u.UserName, o => o.MapFrom(r => r.Username))
                .ForMember(u => u.Email, o => o.MapFrom(r => r.Email))
                .ForMember(u => u.PhoneNumber, o => o.MapFrom(r => r.PhoneNumber))
                .ReverseMap();

            CreateMap<CreateUser.Command, AppUser>()
               //.IgnoreAllUnmapped()
               .ForMember(u => u.UserName, o => o.MapFrom(r => r.Username))
               .ForMember(u => u.Email, o => o.MapFrom(r => r.Email))
               .ForMember(u => u.PhoneNumber, o => o.MapFrom(r => r.PhoneNumber))
               .ReverseMap();
        }
    }
}