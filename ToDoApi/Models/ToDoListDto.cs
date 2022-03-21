using System.Collections.Generic;
using System;

namespace ToDoApi.Models
{
    public class ToDoListDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        //public Guid UserId { get; set; }

        //public IList<ToDoItemDto> ToDoItems { get; private set; } = new List<ToDoItemDto>();
    }
}