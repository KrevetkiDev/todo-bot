using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Bot.Interfaces;
using Krevetki.ToDoBot.Bot.Pipes.Base;
using Krevetki.ToDoBot.Bot.Pipes.Callback;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Telegram.Bot;
using Telegram.Bot.Types;

using CallbackData = Krevetki.ToDoBot.Domain.Entities.CallbackData;

namespace Krevetki.ToDoBot.Bot.Services;

public record CallbackDataReceiver(
    IRepository Repository,
    IEnumerable<IPipe<CallbackQueryPipeContext>> Pipes,
    ILogger<ICallbackReceiver> Logger)
    : ICallbackReceiver
{
    public async Task ReceiveAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Received message {CallbackQuery}", update.CallbackQuery);
        if (Guid.TryParse(update.CallbackQuery?.Data, out var callbackDataId))
        {
            await using var transaction = await Repository.BeginTransactionAsync<CallbackData>(cancellationToken);

            var callbackDataDb = await transaction.Set.FirstOrDefaultAsync(x => x.Id == callbackDataId, cancellationToken);

            if (callbackDataDb == null) return;

            var callbackData =
                JsonConvert.DeserializeObject<Krevetki.ToDoBot.Application.Common.Models.CallbackData>(callbackDataDb.JsonData);

            var pipeContext = new CallbackQueryPipeContext { Data = callbackDataDb.JsonData, DataType = callbackData!.CallbackType };

            foreach (var pipe in Pipes)
            {
                await pipe.HandleAsync(pipeContext, cancellationToken);
            }
        }
    }
}
