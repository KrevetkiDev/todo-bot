using Krevetki.ToDoBot.Application.Common.Models;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.TodayList;

public class TodayListQuery : IRequest<List<Message>>
{
    public long TelegramId { get; set; }
}
