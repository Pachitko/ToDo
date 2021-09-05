using AutoMapper;
using Core.Domain.Entities;
using ToDoApi.Models;

namespace ToDoApi.MapperProfiles
{
    public class ToDoListsProfile : Profile
    {
        public ToDoListsProfile()
        {
            CreateMap<ToDoList, ToDoListDto>().ReverseMap();
        }
    }
}