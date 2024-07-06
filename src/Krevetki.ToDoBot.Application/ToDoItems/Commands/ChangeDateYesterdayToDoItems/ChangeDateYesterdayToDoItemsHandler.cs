using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.Commands.ChangeDateYesterdayToDoItems;

public record ChangeDateYesterdayToDoItemsHandler(IRepository Repository, IMessageService MessageService)
    : IRequestHandler<ChangeDateYesterdayToDoItemsCommand>
{
    public async Task Handle(ChangeDateYesterdayToDoItemsCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<ToDoItem>(cancellationToken);

        var toDoItems = transaction.Set.Where(x => request.ToDoItemIds.Contains(x.Id));

        foreach (var toDoItem in toDoItems)
        {
            toDoItem.DateTimeToStart = toDoItem.DateTimeToStart.Date.Add(DateTime.UtcNow.Date - toDoItem.DateTimeToStart.Date);
        }

        await transaction.CommitAsync(cancellationToken);
    }
}
