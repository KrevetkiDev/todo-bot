using Krevetki.ToDoBot.Application.Common.Models;

namespace Krevetki.ToDoBot.Application.Common.Interfaces;

public interface IMessageService
{
    Task SendMessageAsync(Message message, long chatId, CancellationToken cancellationToken);

    Task DeleteMessageAsync(int messageId, long chatId, CancellationToken cancellationToken);
}
