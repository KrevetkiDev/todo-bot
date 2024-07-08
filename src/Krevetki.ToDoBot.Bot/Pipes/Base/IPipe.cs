namespace Krevetki.ToDoBot.Bot.Pipes.Base;

public interface IPipe<in TContext>
    where TContext : PipeContextBase
{
    Task HandleAsync(TContext context, CancellationToken cancellationToken);
}
