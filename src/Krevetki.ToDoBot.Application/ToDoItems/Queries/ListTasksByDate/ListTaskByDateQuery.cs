using Krevetki.ToDoBot.Application.Common.Models;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.Queries.ListTasksByDate;

public class ListTaskByDateQuery : UserRequest,IRequest
{

    public DateOnly Date { get; set; }
}
