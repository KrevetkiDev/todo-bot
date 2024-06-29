using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Bot.Interfaces;
using Krevetki.ToDoBot.Bot.Pipes.Base;
using Krevetki.ToDoBot.Bot.Pipes.Callback;
using Krevetki.ToDoBot.Bot.Pipes.Command;
using Krevetki.ToDoBot.Bot.Services;
using Krevetki.ToDoBot.Infrastructure.Services;

namespace Krevetki.ToDoBot.Bot;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        return services.AddSingleton<ITelegramService, TelegramService>()
                       .AddSingleton<ICallbackDataSaver, CallbackDataSaver>()
                       .AddSingleton<IMessageService, MessageService>()
                       .AddSingleton<ITelegramClientHolder, TelegramClientHolder>()
                       .AddSingleton<IReceiverService, ReceiverService>()
                       .AddSingleton<IMessageReceiver, MessageReceiver>()
                       .AddSingleton<INotificationService, NotificationService>()
                       .AddSingleton<ICallbackReceiver, CallbackDataReceiver>()
                       .AddSingleton<IPipe<PipeContext>, StartTaskCommandPipe>()
                       .AddSingleton<IPipe<PipeContext>, HelpQueryPipe>()
                       .AddSingleton<IPipe<PipeContext>, NewToDoCommandPipe>()
                       .AddSingleton<IPipe<PipeContext>, TodayListQueryPipe>()
                       .AddSingleton<IPipe<CallbackQueryPipeContext>, ChangeNotificationStatusPipe>()
                       .AddSingleton<IPipe<CallbackQueryPipeContext>, ChangeToDoItemStatusCommandPipe>()
                       .AddHostedService<NotificationService>(p => (p.GetRequiredService<INotificationService>() as NotificationService)!)
                       .AddHostedService<TelegramService>(p => (p.GetRequiredService<ITelegramService>() as TelegramService)!);
    }
}
