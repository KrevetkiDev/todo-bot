using Krevetki.ToDoBot.Bot.Pipes;

namespace TodoBot.Bot.Pipes.Base;

public interface ICallbackQueryPipe
{
    Task HandleAsync(CallbackQueryPipeContext context, CancellationToken cancellationToken);
}
