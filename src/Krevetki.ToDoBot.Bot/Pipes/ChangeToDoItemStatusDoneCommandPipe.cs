using MediatR;

using ToDoBot.Application.Users.Commands;

using TodoBot.Bot.Pipes.Base;

using ToDoBot.Domain.Enums;

namespace Krevetki.ToDoBot.Bot.Pipes;

public record ChangeToDoItemStatusDoneCommandPipe(IMediator Mediator) : CallbackQueryPipeBase
{
    protected override string ApplicableMessage => "Done";

    protected override async Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
                         new ChangeToDoItemStatusCommand() { ToDoItemId = context.ToDoItemId, ToDoItemStatus = ToDoItemStatus.Done },
                         cancellationToken);

        context.ResponseMessages.Add(result);
    }
}
