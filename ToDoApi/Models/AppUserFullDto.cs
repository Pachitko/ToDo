using System;

namespace ToDoApi.Models
{
    public class AppUserFullDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset? DateOfBirth{ get; set; }
        public DateTimeOffset? DateOfDeath{ get; set; }
    }
}