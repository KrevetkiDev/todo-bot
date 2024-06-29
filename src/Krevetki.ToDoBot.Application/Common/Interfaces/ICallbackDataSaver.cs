namespace Krevetki.ToDoBot.Application.Common.Interfaces;

public interface ICallbackDataSaver
{
    Task<Guid> SaveCallbackDataAsync(object data, CancellationToken cancellationToken);
}
