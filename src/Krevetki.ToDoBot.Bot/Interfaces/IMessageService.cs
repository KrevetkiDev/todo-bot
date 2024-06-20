using Krevetki.ToDoBot.Application.Common.Models;

using Telegram.Bot;

namespace Krevetki.ToDoBot.Bot.Interfaces;

public interface IMessageService
{
    Task SendMessageAsync(ITelegramBotClient client, Message message, long chatId, CancellationToken cancellationToken);

    Task DeleteMessageAsync(ITelegramBotClient client, int messageId, long chatId, CancellationToken cancellationToken);
}
