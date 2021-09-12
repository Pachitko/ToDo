using Core.Application.Extensions;
using Core.Domain.Entities;
using ToDoApi.Models;
using AutoMapper;

namespace ToDoApi.MapperProfiles
{
    public class UsersProfile : Profile
    {
        public override string ProfileName => "User";

        public UsersProfile()
        {
            CreateMap<AppUser, AppUserDto>()
                .ForMember(dest => dest.Name, o => o.MapFrom(src => $"{src.UserProfile.FirstName} {src.UserProfile.MiddleName} {src.UserProfile.LastName}"))
                .ForMember(dest => dest.Age, o => o.MapFrom(src => src.UserProfile.DateOfBirth.GetCurrentAge(src.UserProfile.DateOfDeath)));

            CreateMap<AppUser, AppUserFullDto>()
                .ForMember(dest => dest.FirstName, o => o.MapFrom(src => src.UserProfile.FirstName))
                .ForMember(dest => dest.MiddleName, o => o.MapFrom(src => src.UserProfile.MiddleName))
                .ForMember(dest => dest.LastName, o => o.MapFrom(src => src.UserProfile.LastName))
                .ForMember(dest => dest.DateOfBirth, o => o.MapFrom(src => src.UserProfile.DateOfBirth))
                .ForMember(dest => dest.DateOfDeath, o => o.MapFrom(src => src.UserProfile.DateOfDeath));
        }
    }
}