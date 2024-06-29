using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Application.ToDoItems.ChangeToDoItemStatus;

public record ChangeToDoItemStatusHandler(IRepository Repository, IMessageService MessageService)
    : IRequestHandler<ChangeToDoItemStatusCommand, Message>
{
    public async Task<Message> Handle(ChangeToDoItemStatusCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<ToDoItem>(cancellationToken);

        var toDoItem = transaction.Set.FirstOrDefault(x => x.Id == request.ToDoItemId);

        toDoItem.Status = request.ToDoItemStatus;

        var todayTasksList = await transaction.Set
                                              .AsNoTracking()
                                              .Where(
                                                  x => x.DateTimeToStart.Date == DateTime.Now.ToUniversalTime().Date
                                                       && x.Status == ToDoItemStatus.New)
                                              .ToListAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        if (request.ToDoItemStatus != ToDoItemStatus.New)
        {
            await MessageService.DeleteMessageAsync(request.MessageId, request.ChatId, cancellationToken);
        }

        if (todayTasksList.Count == 0)
        {
            return new Message() { Text = Messages.NoTasksTodayMessage };
        }

        return new Message() { Text = Messages.CountTask(todayTasksList.Count - 1) };
    }
}
