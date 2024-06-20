namespace Krevetki.ToDoBot.Bot.Pipes.Callback;

//
// public record ChangeToDoItemStatusNotToBeDoneCommandPipe(IMediator Mediator) : CallbackQueryPipeBase
// {
//     protected override string ApplicableMessage => "NotToBeDone";
//
//     protected override async Task HandleInternal(CallbackQueryPipeContext context, CancellationToken cancellationToken)
//     {
//         var result = await Mediator.Send(
//                          new ChangeToDoItemStatusCommand() { ToDoItemId = context.ToDoItemId, ToDoItemStatus = ToDoItemStatus.NotToBeDone },
//                          cancellationToken);
//
//         context.ResponseMessages.Add(result);
//     }
// }
