using Microsoft.AspNetCore.Identity;
using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider)
    {
        var roleManager =
            serviceProvider.GetRequiredService<
                RoleManager<IdentityRole>>();

        var userManager =
            serviceProvider.GetRequiredService<
                UserManager<ApplicationUser>>();

        string[] roles =
        {
            "Admin",
            "Staff",
            "User"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(role));
            }
        }

        await CreateUser(
            userManager,
            "admin@training.com",
            "Admin123!",
            "Administrator",
            "Admin");

        await CreateUser(
            userManager,
            "staff@training.com",
            "Staff123!",
            "Staff User",
            "Staff");

        await CreateUser(
            userManager,
            "user@training.com",
            "User123!",
            "Normal User",
            "User");
    }

    private static async Task CreateUser(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string fullName,
        string role)
    {
        var user =
            await userManager
                .FindByEmailAsync(email);

        if (user != null)
            return;

        user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName,
            EmailConfirmed = true
        };

        var result =
            await userManager.CreateAsync(
                user,
                password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(
                user,
                role);
        }
    }
}