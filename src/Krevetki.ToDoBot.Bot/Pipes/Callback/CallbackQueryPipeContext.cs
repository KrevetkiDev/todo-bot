using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Bot.Pipes.Base;

namespace Krevetki.ToDoBot.Bot.Pipes.Callback;

public class CallbackQueryPipeContext : PipeContextBase
{
    public CallbackDataType DataType { get; set; }

    public string Data { get; set; }

    public int MessageId { get; set; }
}
