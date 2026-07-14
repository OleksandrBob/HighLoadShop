using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;
using UserService.Persistence.Repositories;

namespace UserService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("UserDatabase"),
                b => b.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName)));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}