using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.Commands.ChangeEveningNotification;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

using Newtonsoft.Json;

namespace Krevetki.ToDoBot.Bot.Pipes.Callback;

public record ChangeEveningNotificationStatusPipe(IMediator Mediator) : CallbackQueryPipeBase
{
    protected override CallbackDataType ApplicableMessage => CallbackDataType.EveningNotificationStatus;

    protected override async Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken)
    {
        var callbackData = JsonConvert.DeserializeObject<CallbackData<ChangeEveningNotificationStatusCommand>>(context.Data);

        if (callbackData != null)
        {
            callbackData.Data.User = context.User;
            await Mediator.Send(callbackData.Data, cancellationToken);
        }
    }
}
