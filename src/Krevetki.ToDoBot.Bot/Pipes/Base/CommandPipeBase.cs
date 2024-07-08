using Krevetki.ToDoBot.Bot.Pipes.Command;

namespace Krevetki.ToDoBot.Bot.Pipes.Base;

public abstract record CommandPipeBase : IPipe<PipeContext>
{
    protected abstract string ApplicableSygnalSymbol { get; }

    public async Task HandleAsync(PipeContext context, CancellationToken cancellationToken)
    {
        if (context.Message.StartsWith(ApplicableSygnalSymbol))
        {
            await HandleInternal(context, cancellationToken);
        }
    }

    protected abstract Task HandleInternal(PipeContext context, CancellationToken cancellationToken);
}
