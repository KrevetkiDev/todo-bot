using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.ChangeToDoItemStatus;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

using Newtonsoft.Json;

namespace Krevetki.ToDoBot.Bot.Pipes.Callback;

public record ChangeToDoItemStatusCommandPipe(IMediator Mediator) : CallbackQueryPipeBase
{
    protected override CallbackDataType ApplicableMessage => CallbackDataType.TaskStatus;

    protected override async Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken)
    {
        var callbackData = JsonConvert.DeserializeObject<CallbackData<ChangeToDoItemStatusCommand>>(context.Data);
        if (callbackData != null)
        {
            callbackData.Data.User = context.User;
            callbackData.Data.MessageId = context.MessageId;
            await Mediator.Send(callbackData.Data, cancellationToken);
        }
    }
}
