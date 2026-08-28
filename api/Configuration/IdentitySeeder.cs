using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Configuration;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<ApiUser>>();

        await CreateUserAsync(
            userManager,
            "admin@hotel.com",
            "Admin123!",
            "Administrator"
        );

        await CreateUserAsync(
            userManager,
            "user@hotel.com",
            "User1234!",
            "User"
        );

        await CreateUserAsync(
            userManager,
            "customer@hotel.com",
            "Customer123!",
            "Customer"
        );
    }

    private static async Task CreateUserAsync(
        UserManager<ApiUser> userManager,
        string email,
        string password,
        string role)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user != null)
            return;

        user = new ApiUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(e => $"{e.Code}: {e.Description}")
            );

            throw new Exception(
                $"Failed to create seed user {email}: {errors}"
            );
        }

        await userManager.AddToRoleAsync(user, role);
    }
}
