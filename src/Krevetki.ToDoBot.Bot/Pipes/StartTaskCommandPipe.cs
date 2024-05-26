using MediatR;
using ToDoBot.Application;
using ToDoBot.Application.Models.Models;
using ToDoBot.Application.Users.Commands.Start;
using TodoBot.Bot.Pipes;
using TodoBot.Bot.Pipes.Base;

namespace Krevetki.ToDoBot.Bot.Pipes;

public record StartMessagePipe(IMediator Mediator) : CommandPipeBase
{
    protected override string ApplicableMessage => Messages.StartMessage;

    protected override async Task HandleInternal(PipeContext context, CancellationToken cancellationToken)
    {
        var response =
            await Mediator.Send(new StartCommand { TelegramId = context.TelegramId, Username = context.Username }, cancellationToken);

        context.ResponseMessages.Add(response);
    }
}