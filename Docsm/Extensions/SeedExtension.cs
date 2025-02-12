using Docsm.Helpers.Enums;
using Docsm.Models;
using Microsoft.AspNetCore.Identity;

namespace Docsm.Extensions
{
    public static  class SeedExtension
    {
        public static async Task UseUserSeed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!roleManager.Roles.Any())
                {
                    foreach (Roles item in Enum.GetValues(typeof(Roles)))
                    {
                       await roleManager.CreateAsync(new IdentityRole(item.ToString()));
                    }
                }
                if (!userManager.Users.Any(x => x.NormalizedUserName == "ADMIN"))
                {
                    User user = new User
                    {
                        Name = "admin",
                        Surname = "admin",
                        UserName = "admin",
                        Email = "admin@gmail.com",
                        ProfileImageUrl = "photo.jpg",
                        EmailConfirmed = true

                    };
                    var createUserResult = await userManager.CreateAsync(user, "1234");
                    if (createUserResult.Succeeded)
                    {
                      
                        await userManager.AddToRoleAsync(user, nameof(Roles.Admin));
                    }
                }
            }
        }
    }
}
