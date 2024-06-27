using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Bot.Interfaces;
using Krevetki.ToDoBot.Bot.Pipes.Base;
using Krevetki.ToDoBot.Bot.Pipes.Command;

using Telegram.Bot.Types;

namespace Krevetki.ToDoBot.Bot.Services;

public record MessageReceiver(IEnumerable<IPipe<PipeContext>> Pipes, IMessageService TelegramService) : IMessageReceiver
{
    public async Task ReceiveAsync(Update update, CancellationToken cancellationToken)
    {
        var context = new PipeContext
                      {
                          TelegramId = update.Message!.From!.Id,
                          Message = update.Message.Text!,
                          Username = update.Message.From.Username,
                          ChatId = update.Message.Chat.Id
                      };

        foreach (var pipe in Pipes)
        {
            await pipe.HandleAsync(context, cancellationToken);
        }

        foreach (var message in context.ResponseMessages)
        {
            await TelegramService.SendMessageAsync(message, update.Message.Chat.Id, cancellationToken);
        }
    }
}
