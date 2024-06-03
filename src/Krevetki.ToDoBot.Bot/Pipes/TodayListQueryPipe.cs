using MediatR;

using ToDoBot.Application.Models;
using ToDoBot.Application.Users.Queries;

using TodoBot.Bot.Pipes;
using TodoBot.Bot.Pipes.Base;

namespace Krevetki.ToDoBot.Bot.Pipes;

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
