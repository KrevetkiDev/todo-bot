using MediatR;

using ToDoBot.Application;
using ToDoBot.Application.Common.Models;
using ToDoBot.Application.Models.Models;
using ToDoBot.Application.Users.Commands.NewToDo;

using TodoBot.Bot.Pipes;
using TodoBot.Bot.Pipes.Base;

namespace Krevetki.ToDoBot.Bot.Pipes;

public record NewToDoCommandPipe(IMediator Mediator) : CommandPipeBase
{
    protected override string ApplicableMessage => Messages.StartNewTaskMessage;

    protected override async Task HandleInternal(PipeContext context, CancellationToken cancellationToken)
    {
        ToDoItemParser toDoItemParser = new ToDoItemParser();
        var result = toDoItemParser.TryParse(context.Message, out var toDoItemDto);
        if (!result)
        {
            context.ResponseMessages.Add(new Message { Text = Messages.AddTodoErrorMessage });
            return;
        }

        var response =
            await Mediator.Send(
                new NewToDoCommand() { TelegramId = context.TelegramId, Username = context.Username, ToDoItemDto = toDoItemDto },
                cancellationToken);

        context.ResponseMessages.Add(response);
    }
}
