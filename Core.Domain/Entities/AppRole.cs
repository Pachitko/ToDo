using Microsoft.AspNetCore.Identity;
using System;

namespace Core.Domain.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole()
        {

        }

        public AppRole(string roleName) : base(roleName)
        {
        }
    }
}