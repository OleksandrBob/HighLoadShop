using FluentValidation;
using OrderService.Application;
using OrderService.Persistence;

namespace OrderService.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddControllers();

        builder.Services.AddAuthorization();

        builder.Services.AddApplication();
        builder.Services.AddPersistence(builder.Configuration);

        // Add FluentValidation
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            await AppMigrator.MigrateDatabase(dbContext, logger);
        }

        app.Run();
    }
}
