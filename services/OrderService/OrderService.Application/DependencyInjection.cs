using System.Reflection;
using OrderService.Application.Common;
using OrderService.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHandlers();

        services.AddScoped<IDispatcher, Dispatcher>();

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        var handlerInterface = typeof(IRequestHandler<,>);

        var handlers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Select(t => new
            {
                Implementation = t,
                Interfaces = t.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() == handlerInterface)
            })
            .Where(x => x.Interfaces.Any());

        foreach (var handler in handlers)
        {
            foreach (var @interface in handler.Interfaces)
            {
                services.AddScoped(@interface, handler.Implementation);
            }
        }

        return services;
    }
}
