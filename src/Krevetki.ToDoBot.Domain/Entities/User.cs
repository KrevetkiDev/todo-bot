using Krevetki.ToDoBot.Domain.Entities.Base;

namespace Krevetki.ToDoBot.Domain.Entities;

public class User : EntityBase
{
    public long TelegramId { get; set; }

    public string? Username { get; set; }

    public List<ToDoItem> Tasks { get; set; } = [];

    public TimeOnly? EveningNotificationTime { get; set; }

    public int TimeZone { get; set; }

    public long ChatId { get; set; }
}
