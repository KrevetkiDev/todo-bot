using Krevetki.ToDoBot.Application.Common.Models;

using MediatR;

namespace Krevetki.ToDoBot.Application.Users.Commands.Start;

public class StartCommand : IRequest<Message>
{
    public long TelegramId { get; set; }

    public string? Username { get; set; }
}
