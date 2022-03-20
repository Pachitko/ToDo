using Core.Domain.Entities;
using System;

namespace ToDoApi.Models
{
    public class ToDoItemDto
    {
        public Guid Id { get; set; }

        public Guid ToDoListId { get; set; }

        public string Title { get; set; }

        public bool? IsCompleted { get; set; }

        public bool? IsImportant { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public RecurrenceDto Recurrence { get; set; }
    }
}