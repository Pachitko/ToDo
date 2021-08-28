using System;

namespace ToDoApi.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; } // first name + last name from UserProfile class
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
