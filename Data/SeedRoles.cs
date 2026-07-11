using Microsoft.AspNetCore.Identity;

namespace MiniTrainingCenterCatalog.Mvc.Data;

public static class SeedRoles
{
    public static async Task InitializeAsync(
        IServiceProvider serviceProvider)
    {
        var roleManager =
            serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

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
    }
}
