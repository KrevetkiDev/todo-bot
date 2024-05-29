namespace ToDoBot.Application.Models.Models;

public class ToDoItemDto
{
    public DateOnly DateToStart { get; set; }

    public TimeOnly TimeToStart { get; set; }

    public string Title { get; set; } = default!;
}
