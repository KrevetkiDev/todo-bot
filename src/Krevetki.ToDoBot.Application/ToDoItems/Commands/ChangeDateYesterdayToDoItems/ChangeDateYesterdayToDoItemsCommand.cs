using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.Commands.ChangeDateYesterdayToDoItems;

public class ChangeDateYesterdayToDoItemsCommand : UserRequest, IRequest
{
    public ICollection<Guid> ToDoItemIds { get; set; } = [];
}
