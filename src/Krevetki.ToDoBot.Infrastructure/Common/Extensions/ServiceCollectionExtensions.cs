using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krevetki.ToDoBot.Infrastructure.Common.Extension;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TOptions : class
    {
        services.AddOptions<TOptions>()
                .Bind(configuration.GetSection(typeof(TOptions).Name));

        return services;
    }
}
