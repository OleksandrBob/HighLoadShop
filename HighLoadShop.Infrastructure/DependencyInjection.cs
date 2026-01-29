using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HighLoadShop.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add cross-cutting concerns here
        // Example: AWS SQS, Email services, External APIs, etc.
        
        // Configure logging
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });

        // Future: Add AWS SQS
        // services.AddAWSService<IAmazonSQS>();
        
        return services;
    }
}