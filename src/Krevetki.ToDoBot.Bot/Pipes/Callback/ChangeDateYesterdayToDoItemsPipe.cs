using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.Commands.ChangeDateYesterdayToDoItems;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

using Newtonsoft.Json;

namespace Krevetki.ToDoBot.Bot.Pipes.Callback;

public record ChangeDateYesterdayToDoItemsPipe(IMediator Mediator) : CallbackQueryPipeBase(Mediator)
{
    protected override CallbackDataType ApplicableMessage => CallbackDataType.YesterdayTasksMoveToday;

    protected override async Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken) =>
        await HandleInternal<ChangeDateYesterdayToDoItemsCommand>(context, cancellationToken);
}
