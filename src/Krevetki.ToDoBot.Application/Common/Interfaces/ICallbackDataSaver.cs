namespace Krevetki.ToDoBot.Application.Common.Interfaces;

public interface ICallbackDataSaver
{
    Task<Guid> SaveCallbackDataMethod(object data, CancellationToken cancellationToken);
}
