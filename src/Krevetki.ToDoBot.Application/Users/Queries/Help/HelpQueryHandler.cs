using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;

using MediatR;

namespace Krevetki.ToDoBot.Application.Users.Queries.Help;

public record NewTaskQueryHandler(IMessageService MessageService) : IRequestHandler<HelpTaskQuery>
{
    public async Task Handle(HelpTaskQuery request, CancellationToken cancellationToken) =>
        await MessageService.SendMessageAsync(
            new Message() { Text = Messages.HelpMessage },
            request.User.ChatId,
            cancellationToken);
}
