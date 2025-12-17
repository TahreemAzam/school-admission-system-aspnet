using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace SchoolWebsite1.Data
{
    public static class SeedAdmin
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            // Get UserManager and RoleManager
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminRole = "Admin";
            string adminEmail = "admin@admission.com"; // <-- change to your admin email
            string adminPassword = "Admin@123";      // <-- change to your admin password

            // 1. Check if role exists, if not create
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // 2. Check if admin user exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true // Email confirmed automatically
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors));
                }

                // 3. Assign Admin role
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }
}
