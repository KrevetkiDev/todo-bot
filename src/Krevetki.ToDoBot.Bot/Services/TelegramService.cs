using Krevetki.ToDoBot.Application.Common;
using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Bot.Interfaces;
using Krevetki.ToDoBot.Infrastructure.Options;

using Microsoft.Extensions.Options;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Krevetki.ToDoBot.Bot.Services;

public class TelegramService : ITelegramService, IHostedService
{
    private readonly ITelegramBotClient _client;

    private readonly ILogger<ITelegramService> _logger;

    private readonly IReceiverService _receiverService;

    private readonly List<BotCommand> _commands =
    [
        new() { Command = Commands.HelpCommand, Description = "Описание команд" },

        new() { Command = Commands.TodayListCommand, Description = "Список дел на сегодня" },
    ];

    public TelegramService(
        IOptions<TelegramOptions> options,
        ILogger<ITelegramService> logger,
        IReceiverService receiverService)
    {
        _client = new TelegramBotClient(options.Value.Token);
        _logger = logger;
        _receiverService = receiverService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _client.SetMyCommandsAsync(_commands, cancellationToken: cancellationToken);
        _client.StartReceiving(_receiverService.ReceiveAsync, PollingErrorHandler, cancellationToken: cancellationToken);

        _logger.LogInformation("Telegram Service started");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Telegram Service stopped");
        return Task.CompletedTask;
    }

    private Task PollingErrorHandler(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Telegram client error");
        return Task.CompletedTask;
    }
}
