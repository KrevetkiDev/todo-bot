using Krevetki.ToDoBot.Application.Common;
using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Bot.Interfaces;
using Krevetki.ToDoBot.Bot.Pipes.Base;
using Krevetki.ToDoBot.Bot.Pipes.Command;

using Telegram.Bot.Types;

using User = Krevetki.ToDoBot.Domain.Entities.User;

namespace Krevetki.ToDoBot.Bot.Services;

public record MessageReceiver(IEnumerable<IPipe<PipeContext>> Pipes, IMessageService MessageService, IRepository Repository)
    : IMessageReceiver
{
    public async Task ReceiveAsync(Update update, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<User>(cancellationToken);

        var user = transaction.Set.FirstOrDefault(x => x.TelegramId == update.Message.From.Id);

        PipeContext context;

        if (user is null && update.Message?.Text == Commands.StartCommand)
        {
            context = new PipeContext()
                      {
                          User = new User()
                                 {
                                     Username = update.Message.From.Username,
                                     ChatId = update.Message.Chat.Id,
                                     TelegramId = update.Message.From.Id
                                 },
                          Message = update.Message.Text
                      };

            foreach (var pipe in Pipes)
            {
                await pipe.HandleAsync(context, cancellationToken);
            }
        }

        if (user is not null)
        {
            context = new PipeContext { User = user, Message = update.Message.Text! };

            foreach (var pipe in Pipes)
            {
                await pipe.HandleAsync(context, cancellationToken);
            }
        }
    }
}
