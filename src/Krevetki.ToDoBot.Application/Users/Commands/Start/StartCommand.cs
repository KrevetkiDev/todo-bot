using MediatR;

using ToDoBot.Application.Models.Models;

namespace ToDoBot.Application.Users.Commands.Start;

public class StartCommand : IRequest<Message>
{
    public long TelegramId { get; set; }

    public string? Username { get; set; }
}
