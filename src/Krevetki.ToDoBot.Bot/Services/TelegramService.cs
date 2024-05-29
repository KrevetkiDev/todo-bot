using Microsoft.Extensions.Options;

using Telegram.Bot;
using Telegram.Bot.Types;

using ToDoBot.Application.Common.Interfaces;
using ToDoBot.Application.Models;

using TodoBot.Bot.Pipes;
using TodoBot.Bot.Pipes.Base;

using ToDoBot.Infrastructure.Options;

namespace Krevetki.ToDoBot.Bot.Services;

public class TelegramService : ITelegramService, IHostedService
{
    private readonly ITelegramBotClient _client;

    private readonly ILogger<ITelegramService> _logger;

    private readonly IEnumerable<ICommandPipe> _commandPipes;

    private readonly List<BotCommand> _commands =
    [
        new() { Command = Commands.HelpCommand, Description = "Описание команд" },
    ];

    public TelegramService(
        IOptions<TelegramOptions> options,
        ILogger<ITelegramService> logger,
        IEnumerable<ICommandPipe> commandPipes)
    {
        _client = new TelegramBotClient(options.Value.Token);
        _logger = logger;
        _commandPipes = commandPipes;
    }

    private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Received message {Message}", update.Message);

            if (update.Message is { Text: not null })
            {
                var context = new PipeContext
                              {
                                  TelegramId = update.Message.From!.Id,
                                  Message = update.Message.Text!,
                                  Username = update.Message.From.Username
                              };

                foreach (var pipe in _commandPipes)
                {
                    await pipe.HandleAsync(context, cancellationToken);
                }

                foreach (var message in context.ResponseMessages)
                {
                    await _client.SendTextMessageAsync(
                        update.Message.Chat.Id,
                        message.Text,
                        cancellationToken: cancellationToken);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error handling message");
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _client.SetMyCommandsAsync(_commands, cancellationToken: cancellationToken);
        _client.StartReceiving(UpdateHandler, PollingErrorHandler, cancellationToken: cancellationToken);

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
