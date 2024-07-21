using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.ToDoItems.Queries.ListTasksByDate;
using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Application.ToDoItems.Queries.GetTodoitemsByDate;

public record GetTodoItemsByDateHandler(IRepository Repository) : IRequestHandler<GetTodoItemsByDateQuery, List<ToDoItem>>
{
    public async Task<List<ToDoItem>> Handle(GetTodoItemsByDateQuery request, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<ToDoItem>(cancellationToken);

        return await transaction.Set
                                .AsNoTracking()
                                .Where(x => x.UserId == request.UserId && x.DateTimeToStart.Date == request.Date.Date)
                                .ToListAsync(cancellationToken);
    }
}
