using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;
using UserService.Application.Services;

namespace UserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Options.JwtOptions>(configuration.GetSection(Options.JwtOptions.SectionName));
        services.Configure<Options.Auth0Options>(configuration.GetSection(Options.Auth0Options.SectionName));

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuth0IdentityService, Auth0IdentityService>();
        services.AddScoped<IUserApplicationService, UserApplicationService>();

        return services;
    }
}