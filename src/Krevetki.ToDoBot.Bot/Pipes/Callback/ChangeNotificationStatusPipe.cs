using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.Notifications.ChangeNotificationStatus;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

using Newtonsoft.Json;

namespace Krevetki.ToDoBot.Bot.Pipes.Callback;

public record ChangeNotificationStatusPipe(IMediator Mediator) : CallbackQueryPipeBase
{
    protected override CallbackDataType ApplicableMessage => CallbackDataType.NotificationInterval;

    protected override async Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken)
    {
        var callbackData = JsonConvert.DeserializeObject<CallbackData<ChangeNotificationStatusCommand>>(context.Data);
        if (callbackData != null)
        {
            var message = await Mediator.Send(callbackData.Data, cancellationToken);

            context.ResponseMessages.Add(message);
        }
    }
}
