using MediatR;

using ToDoBot.Application.Models.Models;

namespace ToDoBot.Application.Users.Queries;

public class TodayListQuery : IRequest<List<Message>>
{
    public long TelegramId { get; set; }

    public string? Username { get; set; }
}
