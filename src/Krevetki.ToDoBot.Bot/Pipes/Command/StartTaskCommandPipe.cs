using Krevetki.ToDoBot.Application.Common;
using Krevetki.ToDoBot.Application.Users.Commands.Start;
using Krevetki.ToDoBot.Bot.Pipes.Base;

using MediatR;

namespace Krevetki.ToDoBot.Bot.Pipes.Command;

public record StartTaskCommandPipe(IMediator Mediator) : CommandPipeBase
{
    protected override string ApplicableMessage => Commands.StartCommand;

    protected override async Task HandleInternal(PipeContext context, CancellationToken cancellationToken)
    {
        var response =
            await Mediator.Send(
                new StartCommand { TelegramId = context.TelegramId, Username = context.Username, ChatId = context.ChatId },
                cancellationToken);

        context.ResponseMessages.Add(response);
    }
}
