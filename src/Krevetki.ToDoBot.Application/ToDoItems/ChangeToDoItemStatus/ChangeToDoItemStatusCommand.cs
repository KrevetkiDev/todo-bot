using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.ChangeToDoItemStatus;

public class ChangeToDoItemStatusCommand : IRequest<Message>
{
    public ToDoItemStatus ToDoItemStatus { get; set; }

    public long ChatId { get; set; }

    public Guid ToDoItemId { get; set; }

    public int MessageId { get; set; }
}
