using Krevetki.ToDoBot.Application.Common.Models;

using MediatR;

namespace Krevetki.ToDoBot.Application.Users.Commands.NewToDo;

public class NewToDoCommand : IRequest<List<Message>>
{
    public long TelegramId { get; set; }

    public string? Username { get; set; }

    public ToDoItemDto ToDoItemDto { get; set; }
}
