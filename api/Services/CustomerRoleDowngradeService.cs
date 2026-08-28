using api.Data;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

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

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "CustomerRoleDowngradeService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var userManager =
                    scope.ServiceProvider
                        .GetRequiredService<UserManager<ApiUser>>();

                var dbContext =
                    scope.ServiceProvider
                        .GetRequiredService<ApplicationDbContext>();

                var customerRole =
                    await dbContext.Roles
                        .SingleOrDefaultAsync(
                            r => r.Name == "Customer",
                            stoppingToken);

                if (customerRole == null)
                {
                    _logger.LogWarning(
                        "Customer role was not found.");

                    await Task.Delay(
                        TimeSpan.FromMinutes(5),
                        stoppingToken);

                    continue;
                }

                var customers =
                    await dbContext.Users
                        .Where(u =>
                            dbContext.UserRoles.Any(
                                ur =>
                                    ur.UserId == u.Id &&
                                    ur.RoleId == customerRole.Id))
                        .ToListAsync(stoppingToken);

                foreach (var customer in customers)
                {
                    var hasActiveReservation =
                        await dbContext.Reservations
                            .AnyAsync(
                                r =>
                                    r.ApiUserId == customer.Id &&
                                    r.CheckOutDate >= DateTime.UtcNow,
                                stoppingToken);

                    if (hasActiveReservation)
                        continue;

                    if (!await userManager.IsInRoleAsync(
                            customer,
                            "Customer"))
                    {
                        continue;
                    }

                    var removeResult =
                        await userManager.RemoveFromRoleAsync(
                            customer,
                            "Customer");

                    if (!removeResult.Succeeded)
                    {
                        foreach (var error in removeResult.Errors)
                        {
                            _logger.LogError(
                                "Failed to remove Customer role " +
                                "from {Email}: {Code} - {Description}",
                                customer.Email,
                                error.Code,
                                error.Description);
                        }

                        continue;
                    }

                    if (!await userManager.IsInRoleAsync(
                            customer,
                            "User"))
                    {
                        var addResult =
                            await userManager.AddToRoleAsync(
                                customer,
                                "User");

                        if (!addResult.Succeeded)
                        {
                            foreach (var error in addResult.Errors)
                            {
                                _logger.LogError(
                                    "Failed to add User role to " +
                                    "{Email}: {Code} - {Description}",
                                    customer.Email,
                                    error.Code,
                                    error.Description);
                            }

                            continue;
                        }
                    }

                    _logger.LogInformation(
                        "User {Email} demoted from Customer to User.",
                        customer.Email);
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
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

        _logger.LogInformation(
            "CustomerRoleDowngradeService stopped.");
    }
}
