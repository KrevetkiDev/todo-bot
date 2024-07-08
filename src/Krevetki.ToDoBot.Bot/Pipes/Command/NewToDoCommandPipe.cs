using Krevetki.ToDoBot.Application;
using Krevetki.ToDoBot.Application.Common;
using Krevetki.ToDoBot.Application.Common.Helpers;
using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.Commands.NewToDo;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

namespace Krevetki.ToDoBot.Bot.Pipes.Command;

public record NewToDoCommandPipe(IMediator Mediator, IMessageService MessageService) : CommandPipeBase
{
    protected override string ApplicableSygnalSymbol => Messages.StartNewTaskSygnalSymbol;

    protected override async Task HandleInternal(PipeContext context, CancellationToken cancellationToken)
    {
        var toDoItemParser = new ToDoItemParser();
        if (!toDoItemParser.TryParseToDoItem(context.Message, out var toDoItemDto))
        {
            await MessageService.SendMessageAsync(
                new Message { Text = Messages.AddTodoErrorMessage },
                context.User.ChatId,
                cancellationToken);
            return;
        }

        await Mediator.Send(
            new NewToDoCommand() { User = context.User, ToDoItemDto = toDoItemDto },
            cancellationToken);
    }
}
