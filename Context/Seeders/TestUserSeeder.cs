using EnergyEfficiencyBE.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace EnergyEfficiencyBE.Context.Seeders
{
    public class TestUserSeeder
    {
        public static async Task SeedTestUserAsync(RelationalContext applicationContext, UserManager<AuthUser> userManager)
        {
            if (await userManager.FindByEmailAsync(Constants.SuperAdminEmail) == null)
            {
                var user = new AuthUser
                {
                    UserName = Constants.SuperAdminName,
                    Email = Constants.SuperAdminEmail
                };

                var result = await userManager.CreateAsync(user, Constants.SuperAdminPassword);

                if (result.Succeeded)
                {
                    string[] roleNames = { "Admin", "User" };

                    foreach (var roleName in roleNames)
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }

                    if (user is not null)
                    {
                        if (applicationContext.Users.FirstOrDefault(u => user.Id == u.IdentityId) is null)
                        {
                            var applicationUser = new User
                            {
                                Id = Guid.NewGuid(),
                                Name = Constants.SuperAdminName,
                                IdentityId = user.Id,
                                Email = user.Email
                            };

                            applicationContext.Users.Add(applicationUser);
                            await applicationContext.SaveChangesAsync();
                        }
                    }
                }
            }
        }
    }
}
