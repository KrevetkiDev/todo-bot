using Krevetki.ToDoBot.Bot.Pipes.Base;

namespace Krevetki.ToDoBot.Bot.Pipes.Command;

public class PipeContext : PipeContextBase
{
    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Message { get; set; } = default!;
}
