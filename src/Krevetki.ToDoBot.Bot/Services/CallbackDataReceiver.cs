using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Bot.Interfaces;
using Krevetki.ToDoBot.Bot.Pipes.Base;
using Krevetki.ToDoBot.Bot.Pipes.Callback;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Telegram.Bot.Types;

using CallbackData = Krevetki.ToDoBot.Domain.Entities.CallbackData;

namespace Krevetki.ToDoBot.Bot.Services;

public record CallbackDataReceiver(
    IRepository Repository,
    IEnumerable<IPipe<CallbackQueryPipeContext>> Pipes,
    IMessageService MessageService,
    ILogger<ICallbackReceiver> Logger)
    : ICallbackReceiver
{
    public async Task ReceiveAsync(Update update, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Received message {CallbackQuery}", update.CallbackQuery);
        if (Guid.TryParse(update.CallbackQuery?.Data, out var callbackDataId))
        {
            await using var transaction = await Repository.BeginTransactionAsync<CallbackData>(cancellationToken);

            var callbackDataDb = await transaction.Set.FirstOrDefaultAsync(x => x.Id == callbackDataId, cancellationToken);

            if (callbackDataDb == null) return;

            var callbackData =
                JsonConvert.DeserializeObject<Krevetki.ToDoBot.Application.Common.Models.CallbackData>(callbackDataDb.JsonData);

            var pipeContext = new CallbackQueryPipeContext
                              {
                                  Data = callbackDataDb.JsonData,
                                  DataType = callbackData!.CallbackType,
                                  MessageId = update.CallbackQuery.Message.MessageId,
                                  ChatId = update.CallbackQuery.Message.Chat.Id
                              };

            foreach (var pipe in Pipes)
            {
                await pipe.HandleAsync(pipeContext, cancellationToken);
            }

            foreach (var message in pipeContext.ResponseMessages)
            {
                if (update.CallbackQuery.Message != null)
                    await MessageService.SendMessageAsync(message, update.CallbackQuery.Message.Chat.Id, cancellationToken);
            }
        }
    }
}
