using Krevetki.ToDoBot.Domain.Entities.Base;

namespace Krevetki.ToDoBot.Domain.Entities;

public class Notification : EntityBase
{
    public DateTime NotificationTime { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public Guid ToDoItemId { get; set; }

    public ToDoItem ToDoItem { get; set; }
}
