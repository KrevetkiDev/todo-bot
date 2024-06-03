using ToDoBot.Domain.Enums;

namespace ToDoBot.Application.Common.Models;

public class CallbackDataDto
{
    public ToDoItemStatus Status { get; set; }

    public Guid ToDoItemId { get; set; }

    public string CallbackType { get; set; }
}
