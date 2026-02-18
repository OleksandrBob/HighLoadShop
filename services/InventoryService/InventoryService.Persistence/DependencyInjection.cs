using InventoryService.Application.Interfaces;
using InventoryService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Inventory Context Database
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("InventoryDatabase"),
                b => b.MigrationsAssembly(typeof(InventoryDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IInventoryRepository, InventoryRepository>();

        return services;
    }
}
