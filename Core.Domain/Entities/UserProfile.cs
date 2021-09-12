using System;

namespace Core.Domain.Entities
{
    public record UserProfile
    {
        public string FirstName { get; init; }

        public string MiddleName { get; init; }

        public string LastName { get; init; }

        public DateTimeOffset? DateOfBirth { get; set; }
        
        public DateTimeOffset? DateOfDeath { get; set; }
    }
}