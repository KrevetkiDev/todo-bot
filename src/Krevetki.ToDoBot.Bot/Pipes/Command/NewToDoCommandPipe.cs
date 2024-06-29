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
        ToDoItemParser toDoItemParser = new ToDoItemParser();
        if (!toDoItemParser.TryParseToDoItem(context.Message, out var toDoItemDto))
        {
            context.ResponseMessages.Add(new Message { Text = Messages.AddTodoErrorMessage });
            return;
        }

        var response =
            await Mediator.Send(
                new NewToDoCommand() { TelegramId = context.TelegramId, Username = context.Username, ToDoItemDto = toDoItemDto },
                cancellationToken);

        foreach (var message in response)
        {
            context.ResponseMessages.Add(message);
        }
    }
}
