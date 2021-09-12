using Core.Application.Features.Commands.CreateToDoList;
using Core.Domain.Entities;
using AutoMapper;

namespace ToDoApi.MapperProfiles
{
    public class ToDoListsProfile : Profile
    {
        public ToDoListsProfile()
        {
            CreateMap<CreateToDoList.Command, ToDoList>();
        }
    }
}