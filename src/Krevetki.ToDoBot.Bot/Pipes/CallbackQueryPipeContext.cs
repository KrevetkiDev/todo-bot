using ToDoBot.Application.Models.Models;

namespace Krevetki.ToDoBot.Bot.Pipes;

public class CallbackQueryPipeContext
{
    public string? ButtonName { get; set; }

    public Guid ToDoItemId { get; set; }

    public ICollection<Message> ResponseMessages { get; set; } = new List<Message>();
}
