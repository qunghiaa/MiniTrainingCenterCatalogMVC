using Microsoft.AspNetCore.Identity;
using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Data;

public static class SeedAdmin
{
    public static async Task InitializeAsync(
        IServiceProvider serviceProvider)
    {
        var userManager =
            serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        const string email =
            "admin@example.com";

        var user =
            await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = "System Administrator",
                EmailConfirmed = true
            };

            var result =
                await userManager.CreateAsync(
                    user,
                    "Password@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    user,
                    "Admin");
            }
        }
    }
}