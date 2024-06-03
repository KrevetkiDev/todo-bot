using Krevetki.ToDoBot.Bot.Pipes;

namespace TodoBot.Bot.Pipes.Base;

public abstract record CallbackQueryPipeBase : ICallbackQueryPipe
{
    protected abstract string ApplicableMessage { get; }

    public async Task HandleAsync(CallbackQueryPipeContext context, CancellationToken cancellationToken)
    {
        if (context.ButtonName != null && context.ButtonName.StartsWith(ApplicableMessage))
        {
            await HandleInternal(context, cancellationToken);
        }
    }

    protected abstract Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken);
}
