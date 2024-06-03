using ToDoBot.Application.Common.Models;

namespace ToDoBot.Application.Models.Models;

public class Message
{
    public string Text { get; set; }

    public InlineKeyboard Keyboard { get; set; }
}
