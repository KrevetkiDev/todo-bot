namespace Krevetki.ToDoBot.Application.Common.Models;

public class Message
{
    public string Text { get; set; } = null!;

    public InlineKeyboard? Keyboard { get; set; }
}
