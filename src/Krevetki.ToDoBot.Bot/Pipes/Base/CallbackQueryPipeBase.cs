using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Bot.Pipes.Callback;

namespace Krevetki.ToDoBot.Bot.Pipes.Base;

public abstract record CallbackQueryPipeBase : IPipe<CallbackQueryPipeContext>
{
    protected abstract CallbackDataType ApplicableMessage { get; }

    public async Task HandleAsync(CallbackQueryPipeContext context, CancellationToken cancellationToken)
    {
        if (context.DataType == ApplicableMessage)
        {
            await HandleInternal(context, cancellationToken);
        }
    }

    protected abstract Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken);
}
