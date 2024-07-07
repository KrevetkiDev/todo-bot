using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Application.ToDoItems.Commands.ChangeEveningNotification;

public record ChangeEveningStatusHandler(IRepository Repository, IMessageService MessageService)
    : IRequestHandler<ChangeEveningNotificationStatusCommand>
{
    public async Task Handle(ChangeEveningNotificationStatusCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<User>(cancellationToken);
        var user = await transaction.Set.FirstOrDefaultAsync(x => x.Id == request.User.Id, cancellationToken: cancellationToken);

        var newNotificationStatus = request.EveningNotificationStatus;

        user!.EveningNotificationStatus = newNotificationStatus;
        await transaction.CommitAsync(cancellationToken);

        if (newNotificationStatus == EveningNotificationStatus.Active)
        {
            await MessageService.SendMessageAsync(
                new Message() { Text = Messages.EveningNotificationActive },
                request.User.ChatId,
                cancellationToken);
        }

        if (newNotificationStatus == EveningNotificationStatus.Disable)
        {
            await MessageService.SendMessageAsync(
                new Message() { Text = Messages.EveningNotificationNotDisable },
                request.User.ChatId,
                cancellationToken);
        }
    }
}
