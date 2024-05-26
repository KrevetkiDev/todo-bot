using Krevetki.ToDoBot.Bot.Services;
using ToDoBot.Application.Common.Interfaces;

namespace Krevetki.ToDoBot.Bot;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        return services.AddSingleton<ITelegramService, TelegramService>()
            .AddHostedService<TelegramService>(p => (p.GetRequiredService<ITelegramService>() as TelegramService)!);
    }
}