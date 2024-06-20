using Krevetki.ToDoBot.Bot.Interfaces;
using Krevetki.ToDoBot.Bot.Pipes.Base;
using Krevetki.ToDoBot.Bot.Pipes.Command;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Krevetki.ToDoBot.Bot.Services;

public record MessageReceiver(IEnumerable<IPipe<PipeContext>> Pipes, IMessageService MessageService) : IMessageReceiver
{
    public async Task ReceiveAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        var context = new PipeContext
                      {
                          TelegramId = update.Message!.From!.Id, Message = update.Message.Text!, Username = update.Message.From.Username
                      };

        foreach (var pipe in Pipes)
        {
            await pipe.HandleAsync(context, cancellationToken);
        }

        foreach (var message in context.ResponseMessages)
        {
            await MessageService.SendMessageAsync(client, message, update.Message.Chat.Id, cancellationToken);
        }
    }
}
