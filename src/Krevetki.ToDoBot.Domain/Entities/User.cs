using Krevetki.ToDoBot.Domain.Entities.Base;
using Krevetki.ToDoBot.Domain.Enums;

namespace Krevetki.ToDoBot.Domain.Entities;

public class User : EntityBase
{
    public long TelegramId { get; set; }

    public string? Username { get; set; }

    public List<ToDoItem> Tasks { get; set; } = [];

    public EveningNotificationStatus EveningNotificationStatus { get; set; }

    public int TimeZone { get; set; }

    public long ChatId { get; set; }
}
