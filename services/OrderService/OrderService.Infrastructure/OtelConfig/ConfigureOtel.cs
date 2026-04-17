using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace OrderService.Infrastructure;

public static class ConfigureOtel
{
    public static IHostApplicationBuilder AddOpenTelemetryLogging(this IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Logging.ClearProviders();

        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? "OrderService";
        var serviceVersion = configuration["OpenTelemetry:ServiceVersion"] ?? "1.0.0";
        var otlpEndpoint = configuration["OpenTelemetry:Endpoint"];
        var isDevelopment = builder.Environment.IsDevelopment();

        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
            .WithLogging(logging =>
            {
                if (!string.IsNullOrWhiteSpace(otlpEndpoint))
                {
                    logging.AddOtlpExporter(otlp =>
                    {
                        otlp.Endpoint = new Uri(otlpEndpoint);
                        otlp.Protocol = OtlpExportProtocol.Grpc;
                    });
                }

                if (isDevelopment)
                {
                    logging.AddConsoleExporter();
                }
            },
            options =>
            {
                options.IncludeScopes = true;
                options.IncludeFormattedMessage = true;
            });

        return builder;
    }
}
