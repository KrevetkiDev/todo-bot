using Krevetki.ToDoBot.Domain.Entities.Base;
using Krevetki.ToDoBot.Domain.Enums;

namespace Krevetki.ToDoBot.Domain.Entities;

public class ToDoItem : EntityBase
{
    public DateTime DateTimeToStart { get; set; }

    public string Title { get; set; } = default!;

    public ToDoItemStatus Status { get; set; }

    public Guid UserId { get; set; }
}
