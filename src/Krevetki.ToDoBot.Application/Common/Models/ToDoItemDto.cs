namespace ToDoBot.Application.Models.Models;

public class ToDoItemDto
{
    public DateOnly DateToStart { get; set; }

    public TimeOnly TimeToStart { get; set; }

    public string Text { get; set; } = default!;
}
