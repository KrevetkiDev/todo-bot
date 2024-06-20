using Krevetki.ToDoBot.Application.Common.Models;

namespace Krevetki.ToDoBot.Bot.Pipes.Callback;

public class CallbackQueryPipeContext
{
    public CallbackDataType DataType { get; set; }

    public string Data { get; set; }

    public ICollection<Message> ResponseMessages { get; set; } = new List<Message>();
}
