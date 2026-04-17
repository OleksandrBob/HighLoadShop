using Microsoft.Extensions.Hosting;

namespace OrderService.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddOpenTelemetryLogging();

        return builder;
    }
}
