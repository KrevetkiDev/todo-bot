using Krevetki.ToDoBot.Application.Common;
using Krevetki.ToDoBot.Application.ToDoItems.Queries.TodayList;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

namespace Krevetki.ToDoBot.Bot.Pipes.Command;

public record TodayListQueryPipe(IMediator Mediator) : CommandPipeBase
{
    protected override string ApplicableSygnalSymbol => Commands.TodayListCommand;

    protected override async Task HandleInternal(PipeContext context, CancellationToken cancellationToken) =>
        await Mediator.Send(new TodayListQuery { User = context.User }, cancellationToken);
}
