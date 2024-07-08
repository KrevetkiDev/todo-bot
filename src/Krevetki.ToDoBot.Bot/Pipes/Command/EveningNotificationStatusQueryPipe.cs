using Krevetki.ToDoBot.Application.Common;
using Krevetki.ToDoBot.Application.Notifications.Queries.EveningNotificationStatus;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

namespace Krevetki.ToDoBot.Bot.Pipes.Command;

public record EveningNotificationStatusQueryPipe(IMediator Mediator) : CommandPipeBase
{
    protected override string ApplicableSygnalSymbol => Commands.EveningNotification;

    protected override async Task HandleInternal(PipeContext context, CancellationToken cancellationToken) =>
        await Mediator.Send(new EveningNotificationStatusQuery { User = context.User }, cancellationToken);
}
