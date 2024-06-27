using Telegram.Bot.Types;

namespace Krevetki.ToDoBot.Bot.Interfaces;

public interface IReceiverService
{
    Task ReceiveAsync(Update update, CancellationToken cancellationToken);
}
