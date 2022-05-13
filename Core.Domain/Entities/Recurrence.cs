﻿using Core.Domain.Entities.Enums;

namespace Core.Domain.Entities
{
    public class Recurrence
    {
        public int Interval { get; set; }
        public RecurrenceType Type { get; set; }
    }
}