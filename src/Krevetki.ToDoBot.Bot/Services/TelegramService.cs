using Krevetki.ToDoBot.Bot.Pipes;

using Microsoft.Extensions.Options;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using ToDoBot.Application.Common.Interfaces;
using ToDoBot.Application.Common.Models;
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

        new BotCommand() { Command = Commands.TodayListCommand, Description = "Список дел на сегодня" },
    ];

    private readonly IEnumerable<ICallbackQueryPipe> _callbackdataPipes;

    public TelegramService(
        IOptions<TelegramOptions> options,
        ILogger<ITelegramService> logger,
        IEnumerable<ICommandPipe> commandPipes,
        IEnumerable<ICallbackQueryPipe> callbackQueryPipes)

    {
        _client = new TelegramBotClient(options.Value.Token);
        _logger = logger;
        _commandPipes = commandPipes;
        _callbackdataPipes = callbackQueryPipes;
    }

    private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Received message {Message}", update.Message);

            if (update.Message is { Text: not null })
            {
                await OnMessageAsync(update, cancellationToken);
            }

            if (update is { CallbackQuery: not null })
            {
                await OnCallbackQueryAsync(update, cancellationToken);
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

    private async Task OnMessageAsync(Update update, CancellationToken cancellationToken)
    {
        var context = new PipeContext
                      {
                          TelegramId = update.Message!.From!.Id, Message = update.Message.Text!, Username = update.Message.From.Username
                      };

        foreach (var pipe in _commandPipes)
        {
            await pipe.HandleAsync(context, cancellationToken);
        }

        foreach (var message in context.ResponseMessages)
        {
            if (message.Keyboard != null && message.Keyboard.Buttons != null)
            {
                var keyboardMarkup = new InlineKeyboardMarkup(
                    message.Keyboard.Buttons.Select(
                        row => row.Select(b => InlineKeyboardButton.WithCallbackData(b.Title, b.CallbackData))));

                await _client.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    message.Text,
                    replyMarkup: keyboardMarkup,
                    cancellationToken: cancellationToken);
            }
            else
            {
                await _client.SendTextMessageAsync(update.Message.Chat.Id, message.Text, cancellationToken: cancellationToken);
            }
        }
    }

    private async Task OnCallbackQueryAsync(Update update, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received message {CallbackQuery}", update.CallbackQuery);

        if (CallbackdataParser.TryParseCallbackdata(update.CallbackQuery.Data, out var dto))
        {
            var context = new CallbackQueryPipeContext { ButtonName = dto.CallbackType, ToDoItemId = dto.ToDoItemId };

            foreach (var pipe in _callbackdataPipes)
            {
                await pipe.HandleAsync(context, cancellationToken);
            }

            foreach (var message in context.ResponseMessages)
            {
                await _client.SendTextMessageAsync(
                    update.CallbackQuery.Message.Chat.Id,
                    message.Text,
                    cancellationToken: cancellationToken);
            }

            if (update.CallbackQuery.Data.StartsWith("Done") || update.CallbackQuery.Data.StartsWith("NotToBeDone"))
            {
                await DeleteMessageAsync(update.CallbackQuery.Message!.Chat.Id, update.CallbackQuery.Message.MessageId);
            }
        }
    }

    private async Task DeleteMessageAsync(long chatId, int messageId)
    {
        try
        {
            await _client.DeleteMessageAsync(chatId, messageId);
            Console.WriteLine("Message deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
