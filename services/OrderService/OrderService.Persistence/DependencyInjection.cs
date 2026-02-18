using OrderService.Application.Interfaces;
using OrderService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Order Context Database
        services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("OrderDatabase"),
                b => b.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
