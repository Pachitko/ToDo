using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;

namespace Core.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        // OnModelCreating: b.HasMany(e => e.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
        //public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        //public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        //public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        //public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }

        public UserProfile UserProfile { get; set; }
        public IList<ToDoList> ToDoLists { get; set; } = new List<ToDoList>();

        public AppUser()
        {

        }

        public AppUser(string userName) : base(userName)
        {

        }
    }
}