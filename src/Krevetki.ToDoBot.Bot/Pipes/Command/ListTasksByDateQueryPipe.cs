using Krevetki.ToDoBot.Application;
using Krevetki.ToDoBot.Application.Common.Helpers;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.TodayList;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

namespace Krevetki.ToDoBot.Bot.Pipes.Command;

public record ListTaskByDatePipe(IMediator Mediator) : CommandPipeBase
{
    protected override string ApplicableSygnalSymbol => Messages.ListTasksByDateSignalSymbol;

    protected override async Task HandleInternal(PipeContext context, CancellationToken cancellationToken)
    {
        var toDoItemParser = new DateParser();
        if (!toDoItemParser.TryParseDate(context.Message, out var date))
        {
            await Mediator.Send(new Message { Text = Messages.AddTodoErrorMessage }, cancellationToken);
            return;
        }

        await Mediator.Send(
            new ListTaskByDateQuery() { User = context.User, Date = date },
            cancellationToken);
    }
}
