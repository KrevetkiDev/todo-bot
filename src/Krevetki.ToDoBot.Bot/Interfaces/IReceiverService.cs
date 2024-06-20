using Telegram.Bot;
using Telegram.Bot.Types;

namespace Krevetki.ToDoBot.Bot.Interfaces;

public interface IReceiverService
{
    Task ReceiveAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken);
}
