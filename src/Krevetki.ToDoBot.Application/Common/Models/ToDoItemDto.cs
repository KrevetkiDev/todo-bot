namespace Krevetki.ToDoBot.Application.Common.Models;

public class ToDoItemDto
{
    public DateTime DateTimeToStart { get; set; }

    public string Title { get; set; } = default!;
}
