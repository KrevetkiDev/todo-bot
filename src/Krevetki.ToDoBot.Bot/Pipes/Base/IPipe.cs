namespace Krevetki.ToDoBot.Bot.Pipes.Base;

public interface IPipe<in TContext>
{
    Task HandleAsync(TContext context, CancellationToken cancellationToken);
}
