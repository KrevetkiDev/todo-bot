using MediatR;

using ToDoBot.Application.Models.Models;
using ToDoBot.Domain;

namespace ToDoBot.Application.Users.Queries.NewTask;

public class NewTaskQueryHandler(IRepository Repository) : IRequestHandler<HelpTaskQuery, Message>
{
    public async Task<Message> Handle(HelpTaskQuery request, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<User>(cancellationToken);

        return new Message { Text = Messages.HelpMessage };
    }
}
