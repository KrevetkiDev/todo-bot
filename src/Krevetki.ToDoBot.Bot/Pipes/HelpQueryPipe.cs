using MediatR;

using ToDoBot.Application.Models;
using ToDoBot.Application.Users.Queries.NewTask;

using TodoBot.Bot.Pipes;
using TodoBot.Bot.Pipes.Base;

namespace Krevetki.ToDoBot.Bot.Pipes;

public record HelpQueryPipe(IMediator Mediator) : CommandPipeBase
{
    protected override string ApplicableMessage => Commands.HelpCommand;

    protected override async Task HandleInternal(PipeContext context, CancellationToken cancellationToken)
    {
        var response =
            await Mediator.Send(new HelpTaskQuery { TelegramId = context.TelegramId, Username = context.Username }, cancellationToken);

        context.ResponseMessages.Add(response);
    }
}
