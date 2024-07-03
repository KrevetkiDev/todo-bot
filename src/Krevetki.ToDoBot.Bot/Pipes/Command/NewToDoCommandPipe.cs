using Krevetki.ToDoBot.Application;
using Krevetki.ToDoBot.Application.Common.Helpers;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.NewToDo;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

namespace Krevetki.ToDoBot.Bot.Pipes.Command;

public record NewToDoCommandPipe(IMediator Mediator) : CommandPipeBase
{
    protected override string ApplicableMessage => Messages.StartNewTaskMessage;

    protected override async Task HandleInternal(PipeContext context, CancellationToken cancellationToken)
    {
        var toDoItemParser = new ToDoItemParser();
        if (!toDoItemParser.TryParseToDoItem(context.Message, out var toDoItemDto))
        {
            await Mediator.Send(new Message { Text = Messages.AddTodoErrorMessage }, cancellationToken);
            return;
        }

        await Mediator.Send(
            new NewToDoCommand() { User = context.User, ToDoItemDto = toDoItemDto },
            cancellationToken);
    }
}
