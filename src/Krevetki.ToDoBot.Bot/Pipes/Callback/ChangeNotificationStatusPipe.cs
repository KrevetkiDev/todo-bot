using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.Notifications.Commands.ChangeNotificationStatus;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

using Newtonsoft.Json;

namespace Krevetki.ToDoBot.Bot.Pipes.Callback;

public record ChangeNotificationStatusPipe(IMediator Mediator) : CallbackQueryPipeBase(Mediator)
{
    protected override CallbackDataType ApplicableMessage => CallbackDataType.NotificationInterval;

    protected override async Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken) =>
        await HandleInternal<ChangeNotificationStatusCommand>(context, cancellationToken);
}
