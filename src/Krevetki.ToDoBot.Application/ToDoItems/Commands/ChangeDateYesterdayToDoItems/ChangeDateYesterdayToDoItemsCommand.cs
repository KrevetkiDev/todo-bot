using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.Commands.ChangeDateYesterdayToDoItems;

public class ChangeDateYesterdayToDoItemsCommand : IRequest
{
    public User User { get; set; }

    public ICollection<Guid> ToDoItemIds { get; set; } = [];
}
