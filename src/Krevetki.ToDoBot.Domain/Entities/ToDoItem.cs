using ToDoBot.Domain.Enums;

namespace ToDoBot.Domain.Entities;

public class ToDoItem : EntityBase
{
    public DateOnly DateToStart { get; set; }

    public TimeOnly TimeToStart { get; set; }

    public string Text { get; set; } = default!;

    public ToDoItemStatus Status { get; set; }
}
