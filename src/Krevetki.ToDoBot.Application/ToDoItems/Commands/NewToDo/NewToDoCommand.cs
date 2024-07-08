using Krevetki.ToDoBot.Application.Common.Models;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.Commands.NewToDo;

public class NewToDoCommand : UserRequest, IRequest
{
    public ToDoItemDto ToDoItemDto { get; set; }

}
