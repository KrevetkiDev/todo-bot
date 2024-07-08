using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.Commands.ChangeDateYesterdayToDoItems;
using Krevetki.ToDoBot.Bot.Pipes.Callback;

using MediatR;

using Newtonsoft.Json;

namespace Krevetki.ToDoBot.Bot.Pipes.Base;

public abstract record CallbackQueryPipeBase(IMediator Mediator) : IPipe<CallbackQueryPipeContext>
{
    protected abstract CallbackDataType ApplicableMessage { get; }

    public async Task HandleAsync(CallbackQueryPipeContext context, CancellationToken cancellationToken)
    {
        if (context.DataType == ApplicableMessage)
        {
            await HandleInternal(context, cancellationToken);
        }
    }

    protected abstract Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken);

    protected virtual async Task HandleInternal<T>(CallbackQueryPipeContext context, CancellationToken cancellationToken) where T : UserRequest
    {
        var callbackData = JsonConvert.DeserializeObject<CallbackData<T>>(context.Data);

        if (callbackData != null)
        {
            callbackData.Data.User = context.User;
            await Mediator.Send(callbackData.Data, cancellationToken);
        }
    }

}
