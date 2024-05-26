

using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using ToDoBot.Application;
using ToDoBot.Application.Common.Interfaces;
using ToDoBot.Application.Models;
using TodoBot.Bot.Pipes.Base;
using ToDoBot.Infrastructure.Options;

namespace Krevetki.ToDoBot.Bot.Services;

public class TelegramService: ITelegramService, IHostedService
{
    private readonly ITelegramBotClient _client;
    private readonly ILogger<ITelegramService> _logger;
    private readonly IEnumerable<ICommandPipe> _commandPipes;
    
    private readonly List<BotCommand> _commands =
    [
        new()
        {
            Command = Commands.NewTaskCommand,
            Description = "Создать задачу"
        },
        
        
    ];
    
    public TelegramService(IOptions<TelegramOptions> options, ILogger<ITelegramService> logger,
        IEnumerable<ICommandPipe> commandPipes)
    {
        _client = new TelegramBotClient(options.Value.Token);
        _logger = logger;
        _commandPipes = commandPipes;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Telegram Service stopped");
        return Task.CompletedTask;
    }
    
    private Task PollingErrorHandler(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Telegram client error");
        return Task.CompletedTask;
    }
}