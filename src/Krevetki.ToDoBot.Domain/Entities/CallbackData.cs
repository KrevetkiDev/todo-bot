using Krevetki.ToDoBot.Domain.Entities.Base;

namespace Krevetki.ToDoBot.Domain.Entities;

public class CallbackData : EntityBase
{
    public string JsonData { get; set; } = null!;
}
