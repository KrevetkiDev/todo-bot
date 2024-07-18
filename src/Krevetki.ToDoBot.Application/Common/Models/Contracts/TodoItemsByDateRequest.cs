namespace Krevetki.ToDoBot.Application.Common.Models.Contracts;

public class TodoItemsByDateRequest
{
    public Guid UserId { get; set; }

    public DateOnly DateTime { get; set; }
}
