using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.Queries.GetTodoitemsByDate;

public class GetTodoItemsByDateQuery : IRequest<List<ToDoItem>>
{
    public Guid UserId { get; set; }

    public DateTime Date { get; set; }
}
