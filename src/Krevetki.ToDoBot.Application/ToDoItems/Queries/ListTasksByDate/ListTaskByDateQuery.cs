using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.TodayList;

public class ListTaskByDateQuery : IRequest
{
    public User User { get; set; }

    public DateOnly Date { get; set; }
}
