using Krevetki.ToDoBot.Bot.Pipes;
using Krevetki.ToDoBot.Bot.Services;

using ToDoBot.Application.Common.Interfaces;

using TodoBot.Bot.Pipes.Base;

namespace Krevetki.ToDoBot.Bot;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        return services.AddSingleton<ITelegramService, TelegramService>()
                       .AddSingleton<ICommandPipe, StartTaskCommandPipe>()
                       .AddSingleton<ICommandPipe, HelpQueryPipe>()
                       .AddSingleton<ICommandPipe, NewToDoCommandPipe>()
                       .AddSingleton<ICommandPipe, TodayListQueryPipe>()
                       .AddSingleton<ICallbackQueryPipe, ChangeToDoItemStatusDoneCommandPipe>()
                       .AddSingleton<ICallbackQueryPipe, ChangeToDoItemStatusNotToBeDoneCommandPipe>()
                       .AddHostedService<TelegramService>(p => (p.GetRequiredService<ITelegramService>() as TelegramService)!);
    }
}
