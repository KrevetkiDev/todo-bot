namespace Krevetki.ToDoBot.Application.Common.Interfaces;

public interface ICallbackDataSaver
{
    /// <summary>
    /// Сохраняет объект как CallbackData в виде json
    /// </summary>
    /// <param name="data">Обьект который упакуется в CallbckData<T></param>
    /// <param name="cancellationToken"></param>
    /// <returns>id callbackData</returns>
    Task<Guid> SaveCallbackDataAsync(object data, CancellationToken cancellationToken);
}
