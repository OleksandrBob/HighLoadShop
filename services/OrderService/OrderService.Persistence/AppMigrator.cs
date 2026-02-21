using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace OrderService.Persistence;

public static class AppMigrator
{
    public static async Task MigrateDatabase(DbContext dbContext, ILogger logger)
    {
        var retryCount = 0;
        var maxRetries = 5;
        var delay = TimeSpan.FromSeconds(5);
        while (retryCount < maxRetries)
        {
            try
            {
                logger.LogInformation("Applying migrations... Attempt {RetryCount}", retryCount + 1);
                dbContext.Database.Migrate();
                logger.LogInformation("Migrations applied successfully");
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error applying migrations. Retrying in {Delay} seconds...", delay.TotalSeconds);
                retryCount++;
                Thread.Sleep(delay);
            }
        }
        if (retryCount == maxRetries)
        {
            logger.LogCritical("Failed to apply migrations after {MaxRetries} attempts. Application will start without database updates.", maxRetries);
        }
    }
}

