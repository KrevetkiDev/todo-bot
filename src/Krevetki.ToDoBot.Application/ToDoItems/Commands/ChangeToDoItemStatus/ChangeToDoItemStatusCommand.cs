using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.Commands.ChangeToDoItemStatus;

public class ChangeToDoItemStatusCommand : UserRequest, IRequest
{
    public ToDoItemStatus ToDoItemStatus { get; set; }

    public Guid ToDoItemId { get; set; }

    public int MessageId { get; set; }
}
