using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Application.Users.Commands.ChangeToDoItemStatus;

public record ChangeToDoItemStatusHandler(IRepository Repository) : IRequestHandler<ChangeToDoItemStatusCommand, Message>
{
    public async Task<Message> Handle(ChangeToDoItemStatusCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<ToDoItem>(cancellationToken);

        var toDoItem = transaction.Set.FirstOrDefault(x => x.Id == request.ToDoItemId);

        var todayTasksList = await transaction.Set
                                              .AsNoTracking()
                                              .Where(
                                                  x => x.DateToStart == DateOnly.FromDateTime(DateTime.Now)
                                                       && x.Status == ToDoItemStatus.New)
                                              .ToListAsync(cancellationToken);

        toDoItem.Status = request.ToDoItemStatus;

        await transaction.CommitAsync(cancellationToken);

        if (todayTasksList.Count == 0)
        {
            return new Message() { Text = Messages.NoTasksTodayMessage };
        }

        return new Message() { Text = Messages.CountTask(todayTasksList.Count - 1) };
    }
}
