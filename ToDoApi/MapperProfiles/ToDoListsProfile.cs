using Core.Domain.Entities;
using ToDoApi.Models;
using AutoMapper;

namespace ToDoApi.MapperProfiles
{
    public class ToDoListsProfile : Profile
    {
        public ToDoListsProfile()
        {
            CreateMap<ToDoList, ToDoListDto>();
        }
    }
}