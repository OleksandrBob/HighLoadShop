using FluentValidation;
using UserService.Application;
using UserService.Persistence;

namespace UserService.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddControllers();
        builder.Services.AddAuthorization();

        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);

        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseAuthorization();
        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            await AppMigrator.MigrateDatabase(dbContext, logger);
        }

        app.Run();
    }
}