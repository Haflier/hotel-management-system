using api.Data;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class CustomerRoleDowngradeService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CustomerRoleDowngradeService> _logger;

    public CustomerRoleDowngradeService(
        IServiceProvider serviceProvider,
        ILogger<CustomerRoleDowngradeService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CustomerRoleDowngradeService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var userManager =
                    scope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();

                var dbContext =
                    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Get all users in "Customer" role
                var customerRole = await dbContext.Roles
                    .FirstOrDefaultAsync(r => r.Name == "Customer", stoppingToken);

                if (customerRole is not null)
                {
                    var customers = await dbContext.Users
                        .Where(u => dbContext.UserRoles.Any(
                            ur => ur.UserId == u.Id &&
                                  ur.RoleId == customerRole.Id))
                        .ToListAsync(stoppingToken);

                    foreach (var customer in customers)
                    {
                        // Get the user's active reservations
                        var activeReservations = await dbContext.Reservations
                            .Where(r =>
                                r.ApiUserId == customer.Id &&
                                r.CheckOutDate >= DateTime.UtcNow)
                            .ToListAsync(stoppingToken);

                        // If no active/future reservations, downgrade role
                        if (!activeReservations.Any())
                        {
                            if (await userManager.IsInRoleAsync(customer, "Customer"))
                            {
                                var result = await userManager.RemoveFromRoleAsync(
                                    customer,
                                    "Customer");

                                if (result.Succeeded)
                                {
                                    if (!await userManager.IsInRoleAsync(customer, "User"))
                                    {
                                        await userManager.AddToRoleAsync(
                                            customer,
                                            "User");
                                    }

                                    _logger.LogInformation(
                                        "User {Email} demoted to User role.",
                                        customer.Email);
                                }
                                else
                                {
                                    _logger.LogWarning(
                                        "Failed to remove Customer role from {Email}: {Errors}",
                                        customer.Email,
                                        string.Join(", ", result.Errors.Select(e => e.Description)));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in CustomerRoleDowngradeService.");
            }

            await Task.Delay(
                TimeSpan.FromMinutes(5),
                stoppingToken);
        }
    }
}
