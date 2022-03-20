using Core.Domain.Entities;
using ToDoApi.Models;
using AutoMapper;
using Core.Domain.Entities.Enums;

namespace ToDoApi.MapperProfiles
{
    public class ToDoItemsProfile : Profile
    {
        public ToDoItemsProfile()
        {
            CreateMap<Recurrence, RecurrenceDto>();
                //.ForMember(to => to.Type, o => o.MapFrom(from => from.Type.ToString()));
            CreateMap<ToDoItem, ToDoItemDto>();
        }
    }
}
