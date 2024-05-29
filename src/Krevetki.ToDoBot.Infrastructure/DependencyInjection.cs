using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ToDoBot.Application;
using ToDoBot.Infrastructure.Options;
using ToDoBot.Infrastructure.Persistence;
using ToDoBot.Infrastructure.Persistence.Repository;

namespace ToDoBot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddServices().AddSettings(configuration).AddDatabase(configuration);
    }

    private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TelegramOptions>().Bind(configuration.GetSection("TelegramOptions"));
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
