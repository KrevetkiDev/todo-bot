namespace Krevetki.ToDoBot.Bot.Pipes.Callback;

// public record ChangeToDoItemStatusDoneCommandPipe(IMediator Mediator) : CallbackQueryPipeBase
// {
//     protected override string ApplicableMessage => "Done";
//
//     protected override async Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken)
//     {
//         var result = await Mediator.Send(
//                          new ChangeToDoItemStatusCommand() { ToDoItemId = context.ToDoItemId, ToDoItemStatus = ToDoItemStatus.Done },
//                          cancellationToken);
//
//         context.ResponseMessages.Add(result);
//     }
// }
