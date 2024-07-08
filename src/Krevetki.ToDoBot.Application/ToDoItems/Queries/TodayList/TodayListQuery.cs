using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.TodayList;

public class TodayListQuery : IRequest
{
    public User User { get; set; }
}
