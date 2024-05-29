using MediatR;

using ToDoBot.Application.Models.Models;

namespace ToDoBot.Application.Users.Queries.NewTask;

public class NewTaskQueryHandler(IRepository Repository) : IRequestHandler<HelpTaskQuery, Message>
{
    public async Task<Message> Handle(HelpTaskQuery request, CancellationToken cancellationToken) => new() { Text = Messages.HelpMessage };
}
