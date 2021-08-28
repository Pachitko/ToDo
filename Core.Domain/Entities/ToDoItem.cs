using System;

namespace Core.Domain.Entities
{
    public class ToDoItem : BaseEntity<Guid>
    {
        public Guid ToDoListId { get; set; }

        public ToDoList ToDoList { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool? IsCompleted { get; set; }

        public bool? IsImportant { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; } 
    }
}
