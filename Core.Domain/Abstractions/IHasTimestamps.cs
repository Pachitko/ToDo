using System;

namespace Core.Domain.Interfaces
{
    public interface IHasTimestamps
    {
        DateTime? Added { get; set; }
        DateTime? Deleted { get; set; }
        DateTime? Modified { get; set; }
    }
}