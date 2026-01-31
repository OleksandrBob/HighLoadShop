using HighLoadShop.Application.InventoryContext.Interfaces;
using HighLoadShop.Application.OrderContext.Interfaces;
using HighLoadShop.Persistence.InventoryContext;
using HighLoadShop.Persistence.InventoryContext.Repositories;
using HighLoadShop.Persistence.OrderContext;
using HighLoadShop.Persistence.OrderContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HighLoadShop.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Order Context Database
        services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("OrderDatabase"),
                b => b.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName)));

        // Inventory Context Database
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("InventoryDatabase"),
                b => b.MigrationsAssembly(typeof(InventoryDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();

        return services;
    }
}