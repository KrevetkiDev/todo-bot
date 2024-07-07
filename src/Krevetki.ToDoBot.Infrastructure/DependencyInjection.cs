using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Infrastructure.Options;
using Krevetki.ToDoBot.Infrastructure.Persistence;
using Krevetki.ToDoBot.Infrastructure.Persistence.Repository;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krevetki.ToDoBot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddServices().AddSettings(configuration).AddDatabase(configuration);
    }

    private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<TelegramOptions>();
        services.ConfigureOptions<EveningNotificationOptions>();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddSingleton<IRepository, Repository>();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContextFactory<DatabaseContext>(
            options =>
                options.UseNpgsql(configuration.GetConnectionString("Database")));
    }
}
