using FluentValidation;
using InventoryService.Application;
using InventoryService.Persistence;

namespace InventoryService.Api;

public class Program
{
    public static void Main(string[] args)
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

        app.Run();
    }
}
