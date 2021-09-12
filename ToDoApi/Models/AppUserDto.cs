using System;

namespace ToDoApi.Models
{
    public class AppUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; } // FirstName + LastName
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? Age { get; set; }
    }
}