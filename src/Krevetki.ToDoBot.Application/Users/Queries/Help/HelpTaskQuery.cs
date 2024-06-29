using Krevetki.ToDoBot.Application.Common.Models;

using MediatR;

namespace Krevetki.ToDoBot.Application.Users.Queries.Help;

public class HelpTaskQuery : IRequest<Message>
{
    public long TelegramId { get; set; }

    public string? Username { get; set; }
}
