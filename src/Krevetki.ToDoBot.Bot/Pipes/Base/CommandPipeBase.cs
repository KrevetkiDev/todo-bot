namespace TodoBot.Bot.Pipes.Base;

public abstract record CommandPipeBase : ICommandPipe
{
    protected abstract string ApplicableMessage { get; }

    public async Task HandleAsync(PipeContext context, CancellationToken cancellationToken)
    {
        if (context.Message == ApplicableMessage)
        {
            await HandleInternal(context, cancellationToken);
        }
    }

    protected abstract Task HandleInternal(PipeContext context, CancellationToken cancellationToken);
}