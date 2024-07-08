using Krevetki.ToDoBot.Application.Common;
using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Application.Users.Commands.Start;

public record StartCommandHandler(IRepository Repository, IMessageService MessageService) : IRequestHandler<StartCommand>
{
    public async Task Handle(StartCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<User>(cancellationToken);

        var user = transaction.Set.AsNoTracking().FirstOrDefault(x => x.TelegramId == request.User.TelegramId);
        if (user == null)
        {
            user = new User
                   {
                       TelegramId = request.User.TelegramId,
                       Username = request.User.Username,
                       ChatId = request.User.ChatId,
                       EveningNotificationStatus = EveningNotificationStatus.Active
                   };
            transaction.Add(user);
            await transaction.CommitAsync(cancellationToken);
        }

        await MessageService.SendMessageAsync(new Message { Text = Messages.StartMessage }, user.ChatId, cancellationToken);
    }
}
