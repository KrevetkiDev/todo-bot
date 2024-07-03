using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.TodayList;

public class TodayListQuery : IRequest<List<Message>>, IRequest<IMessageService>, IRequest
{
    public long TelegramId { get; set; }

    public User User { get; set; }
}
