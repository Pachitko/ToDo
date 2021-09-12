using Core.Application.Features.Commands.CreateToDoItem;
using Core.Domain.Entities;
using AutoMapper;

namespace ToDoApi.MapperProfiles
{
    public class ToDoItemsProfile : Profile
    {
        public ToDoItemsProfile()
        {
            CreateMap<CreateToDoItem.Command, ToDoItem>();
        }
    }
}