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
            CreateMap<AppUser, UserDto>()
                .ForMember(dto => dto.Name, o => o.MapFrom(au => $"{au.UserProfile.FirstName} {au.UserProfile.LastName}"));
        }
    }
}
