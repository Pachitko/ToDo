using System.Collections.Generic;
using System;

namespace Core.Domain.Entities
{
    public class ToDoList : BaseEntity<Guid>
    {
        public string Title { get; set; }

        public Guid UserId { get; set; }

        public AppUser User { get; set; }

        public IList<ToDoItem> ToDoItems { get; private set; } = new List<ToDoItem>();
    }
}