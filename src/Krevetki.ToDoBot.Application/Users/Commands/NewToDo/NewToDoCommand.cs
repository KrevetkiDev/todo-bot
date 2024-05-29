using MediatR;

using ToDoBot.Application.Models.Models;

namespace ToDoBot.Application.Users.Commands.NewToDo;

public class NewToDoCommand : IRequest<Message>
{
    public long? TelegramId { get; set; }

    public string? Username { get; set; }

    public ToDoItemDto ToDoItemDto { get; set; }
}
