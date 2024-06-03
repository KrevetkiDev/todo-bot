using MediatR;

using ToDoBot.Application.Models.Models;
using ToDoBot.Domain.Enums;

namespace ToDoBot.Application.Users.Commands;

public class ChangeToDoItemStatusCommand : IRequest<Message>
{
    public ToDoItemStatus ToDoItemStatus { get; set; }

    public Guid ToDoItemId { get; set; }
}
