using Krevetki.ToDoBot.Application.Common.Interfaces;

using Newtonsoft.Json;

using CallbackData = Krevetki.ToDoBot.Domain.Entities.CallbackData;

namespace Krevetki.ToDoBot.Bot.Services;

public record CallbackDataSaver(IRepository Repository) : ICallbackDataSaver
{
    public async Task<Guid> SaveCallbackDataMethod(object data, CancellationToken cancellationToken)
    {
        await using var transactionCallbackData = await Repository.BeginTransactionAsync<CallbackData>(cancellationToken);
        var callbackData = new CallbackData { JsonData = JsonConvert.SerializeObject(data) };
        transactionCallbackData.Add(callbackData);
        await transactionCallbackData.CommitAsync(cancellationToken);
        return callbackData.Id;
    }
}
