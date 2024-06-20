using Krevetki.ToDoBot.Application.Common.Models;

using MediatR;

namespace Krevetki.ToDoBot.Application.Users.Queries.TodayList;

public class TodayListQuery : IRequest<List<Message>>
{
    public long TelegramId { get; set; }
}
