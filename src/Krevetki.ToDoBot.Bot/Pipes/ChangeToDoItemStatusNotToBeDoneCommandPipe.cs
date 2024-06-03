using MediatR;

using ToDoBot.Application.Users.Commands;

using TodoBot.Bot.Pipes.Base;

using ToDoBot.Domain.Enums;

namespace Krevetki.ToDoBot.Bot.Pipes;

public record ChangeToDoItemStatusNotToBeDoneCommandPipe(IMediator Mediator) : CallbackQueryPipeBase
{
    protected override string ApplicableMessage => "NotToBeDone";

    protected override async Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
                         new ChangeToDoItemStatusCommand() { ToDoItemId = context.ToDoItemId, ToDoItemStatus = ToDoItemStatus.NotToBeDone },
                         cancellationToken);

        context.ResponseMessages.Add(result);
    }
}
