using MediatR;

using ToDoBot.Application.Models.Models;

namespace ToDoBot.Application.Users.Queries.NewTask;

public class HelpTaskQuery : IRequest<Message>
{
    public long? TelegramId { get; set; }

    public string? Username { get; set; }
}
