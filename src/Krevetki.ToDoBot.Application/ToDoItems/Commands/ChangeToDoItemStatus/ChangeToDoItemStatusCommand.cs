using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.ChangeToDoItemStatus;

public class ChangeToDoItemStatusCommand : IRequest
{
    public ToDoItemStatus ToDoItemStatus { get; set; }

    public Guid ToDoItemId { get; set; }

    public int MessageId { get; set; }

    public User User { get; set; }
}
