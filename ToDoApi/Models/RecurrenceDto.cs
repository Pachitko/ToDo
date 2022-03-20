using System;

namespace ToDoApi.Models
{
    public class RecurrenceDto
    {
        public int Interval { get; set; }
        public int Type { get; set; } // todo string
    }
}