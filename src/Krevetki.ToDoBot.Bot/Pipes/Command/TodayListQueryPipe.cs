using Krevetki.ToDoBot.Application.Common;
using Krevetki.ToDoBot.Application.Users.Queries.TodayList;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

namespace Krevetki.ToDoBot.Bot.Pipes.Command;

public record TodayListQueryPipe(IMediator Mediator) : CommandPipeBase
{
    protected override string ApplicableMessage => Commands.TodayListCommand;

    protected override async Task HandleInternal(PipeContext context, CancellationToken cancellationToken)
    {
        var response =
            await Mediator.Send(new TodayListQuery { TelegramId = context.TelegramId }, cancellationToken);

        foreach (var message in response)
        {
            context.ResponseMessages.Add(message);
        }
    }
}
