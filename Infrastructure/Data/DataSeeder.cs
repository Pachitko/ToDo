using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Linq;
using System;

namespace Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(this IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

            var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                if (!context.Roles.Any())
                {
                    AppRole adminRole = new("Admin");
                    await roleManager.CreateAsync(adminRole);

                    AppRole userRole = new("User");
                    await roleManager.CreateAsync(userRole);

                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    UserProfile adminProfile = new() { FirstName = "AdminFirstName", LastName = "AdminLastName" };
                    AppUser admin = new("Admin")
                    {
                        Email = "admin@mail.ru",
                        UserProfile = adminProfile
                    };
                    await userManager.CreateAsync(admin, "Password0");
                    await userManager.AddToRoleAsync(admin, "Admin");

                    UserProfile userProfile = new() { FirstName = "UserFirstName", LastName = "UserLastName" };
                    AppUser user = new("User")
                    {
                        Email = "user@mail.ru",
                        UserProfile = userProfile
                    };
                    await userManager.CreateAsync(user, "Password0");
                    await userManager.AddToRoleAsync(user, "User");

                    context.SaveChanges();
                }
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}