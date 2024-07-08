using Krevetki.ToDoBot.Application;
using Krevetki.ToDoBot.Application.Common.Helpers;
using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.TodayList;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

namespace Krevetki.ToDoBot.Bot.Pipes.Command;

public record ListTaskByDatePipe(IMediator Mediator, IMessageService MessageService) : CommandPipeBase
{
    protected override string ApplicableSygnalSymbol => Messages.ListTasksByDateSignalSymbol;

    protected override async Task HandleInternal(PipeContext context, CancellationToken cancellationToken)
    {
        if (!DateParser.TryParseDate(context.Message, out var date))
        {
            await MessageService.SendMessageAsync(
                new Message { Text = Messages.AddTodoErrorMessage },
                context.User.ChatId,
                cancellationToken);
            return;
        }

        await Mediator.Send(
            new ListTaskByDateQuery() { User = context.User, Date = date },
            cancellationToken);
    }
}
