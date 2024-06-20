using Krevetki.ToDoBot.Bot.Interfaces;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Krevetki.ToDoBot.Bot.Services;

public record ReceiverService(ILogger<IReceiverService> Logger, IMessageReceiver MessageReceiver, ICallbackReceiver CallbackReceiver)
    : IReceiverService
{
    public async Task ReceiveAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Received message {Message}", update.Message);

            if (update.Message is { Text: not null })
                await MessageReceiver.ReceiveAsync(client, update, cancellationToken);

            if (update is { CallbackQuery: not null })
                await CallbackReceiver.ReceiveAsync(client, update, cancellationToken);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error handling message");
        }
    }
}
