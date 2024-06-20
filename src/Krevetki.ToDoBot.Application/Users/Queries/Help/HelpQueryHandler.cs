using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;

using MediatR;

namespace Krevetki.ToDoBot.Application.Users.Queries.Help;

public class NewTaskQueryHandler(IRepository Repository) : IRequestHandler<HelpTaskQuery, Message>
{
    public async Task<Message> Handle(HelpTaskQuery request, CancellationToken cancellationToken) => new() { Text = Messages.HelpMessage };
}
