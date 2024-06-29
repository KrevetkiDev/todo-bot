using Krevetki.ToDoBot.Bot.Interfaces;

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Krevetki.ToDoBot.Bot.Services;

public record ReceiverService(ILogger<IReceiverService> Logger, IMessageReceiver MessageReceiver, ICallbackReceiver CallbackReceiver)
    : IReceiverService
{
    public Task ReceiveAsync(Update update, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Received message {Message}", update.Message);

            return update.Type switch
            {
                UpdateType.Message => MessageReceiver.ReceiveAsync(update, cancellationToken),
                UpdateType.CallbackQuery => CallbackReceiver.ReceiveAsync(update, cancellationToken),
                _ => Task.CompletedTask
            };
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error handling message");
        }

        return Task.CompletedTask;
    }
}
