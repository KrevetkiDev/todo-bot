using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.NewToDo;

public class NewToDoCommand : IRequest<List<Message>>, IRequest
{
    public ToDoItemDto ToDoItemDto { get; set; }

    public User User { get; set; }
}
