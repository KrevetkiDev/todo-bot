using Krevetki.ToDoBot.Application.Common;
using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Bot.Interfaces;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Krevetki.ToDoBot.Bot.Services;

public class TelegramService(
    ITelegramClientHolder clientHolder,
    ILogger<ITelegramService> logger,
    IReceiverService receiverService)
    : ITelegramService, IHostedService
{
    private readonly List<BotCommand> _commands =
    [
        new() { Command = Commands.HelpCommand, Description = "Описание команд" },

        new() { Command = Commands.TodayListCommand, Description = "Список дел на сегодня" },
    ];

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await clientHolder.Client.SetMyCommandsAsync(_commands, cancellationToken: cancellationToken);
        clientHolder.Client.StartReceiving(
            (client, update, ct) => receiverService.ReceiveAsync(update, ct),
            PollingErrorHandler,
            cancellationToken: cancellationToken);

        logger.LogInformation("Telegram Service started");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Telegram Service stopped");
        return Task.CompletedTask;
    }

    private Task PollingErrorHandler(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Telegram client error");
        return Task.CompletedTask;
    }
}
