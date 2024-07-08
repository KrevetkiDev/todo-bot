using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.Notifications.Commands.ChangeEveningNotificationStatus;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

using Newtonsoft.Json;

namespace Krevetki.ToDoBot.Bot.Pipes.Callback;

public record ChangeEveningNotificationStatusPipe(IMediator Mediator) : CallbackQueryPipeBase(Mediator)
{
    protected override CallbackDataType ApplicableMessage => CallbackDataType.EveningNotificationStatus;

    protected override async Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken) =>
        await HandleInternal<ChangeEveningNotificationStatusCommand>(context, cancellationToken);
}
