using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class FactorGenerationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FactorGenerationService> _logger;

        public FactorGenerationService(
            IServiceProvider serviceProvider,
            ILogger<FactorGenerationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("FactorGenerationService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider
                        .GetRequiredService<ApplicationDbContext>();

                    var now = DateTime.UtcNow;

                    var endedReservations = await db.Reservations
                        .Where(r => r.CheckOutDate <= now)
                        .Where(r => !db.Factors.Any(f => f.ReservationId == r.Id))
                        .ToListAsync(stoppingToken);

                    foreach (var reservation in endedReservations)
                    {
                        var orders = await db.Orders
                            .Where(o => o.ApiUserId == reservation.ApiUserId)
                            .Where(o => o.CreatedAt >= reservation.CheckinDate)
                            .Where(o => o.CreatedAt <= reservation.CheckOutDate)
                            .Where(o => o.IsFinalized)
                            .ToListAsync(stoppingToken);

                        var ordersTotalPrice = orders.Sum(o => o.TotalPrice);

                        var finalPrice =
                            reservation.TotalPrice + ordersTotalPrice;

                        var factor = new Factor
                        {
                            ApiUserId = reservation.ApiUserId,
                            ReservationId = reservation.Id,
                            CreatedAt = now,
                            FinalPrice = finalPrice
                        };

                        db.Factors.Add(factor);

                        _logger.LogInformation(
                            "Generated factor for reservation {ReservationId}. Final price: {FinalPrice}",
                            reservation.Id,
                            finalPrice);
                    }

                    if (endedReservations.Count > 0)
                    {
                        await db.SaveChangesAsync(stoppingToken);
                    }
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error generating factors automatically.");
                }

                await Task.Delay(
                    TimeSpan.FromMinutes(5),
                    stoppingToken);
            }

            _logger.LogInformation("FactorGenerationService stopped.");
        }
    }
}
