using ToDoBot.Domain.Entities;

namespace ToDoBot.Domain;

public class User : EntityBase
{
    public long TelegramId { get; set; }

    public string Username { get; set; }

    public List<ToDoItem> Tasks { get; set; } = [];

    public TimeOnly? EveningNotificationTime { get; set; }

    public int TimeZone { get; set; }
}
