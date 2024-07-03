namespace Krevetki.ToDoBot.Application.Common.Interfaces;

public interface ICallbackDataSaver
{
    /// <summary>
    /// Сохраняет объект как CallbackData в виде json
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> SaveCallbackDataAsync(object data, CancellationToken cancellationToken);
}
