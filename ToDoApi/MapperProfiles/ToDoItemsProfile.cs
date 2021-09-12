using Core.Application.Features.Commands.CreateToDoItem;
using Core.Domain.Entities;
using ToDoApi.Models;
using AutoMapper;

namespace ToDoApi.MapperProfiles
{
    public class ToDoItemsProfile : Profile
    {
        public ToDoItemsProfile()
        {
            CreateMap<ToDoItem, ToDoItemDto>();
        }
    }
}
