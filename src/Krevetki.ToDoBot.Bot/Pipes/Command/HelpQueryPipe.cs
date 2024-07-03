using Krevetki.ToDoBot.Application.Common;
using Krevetki.ToDoBot.Application.Users.Queries.Help;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

namespace Krevetki.ToDoBot.Bot.Pipes.Command;

public record HelpQueryPipe(IMediator Mediator) : CommandPipeBase
{
    protected override string ApplicableMessage => Commands.HelpCommand;

    protected override async Task HandleInternal(PipeContext context, CancellationToken cancellationToken) =>
        await Mediator.Send(new HelpTaskQuery { User = context.User }, cancellationToken);
}
