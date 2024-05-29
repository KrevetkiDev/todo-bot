namespace TodoBot.Bot.Pipes.Base;

public interface ICommandPipe
{
    Task HandleAsync(PipeContext context, CancellationToken cancellationToken);
}