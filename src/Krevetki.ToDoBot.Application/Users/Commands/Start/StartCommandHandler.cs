using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoBot.Application.Models.Models;
using ToDoBot.Domain;

namespace ToDoBot.Application.Users.Commands.Start;

public record StartCommandHandler(IRepository Repository) : IRequestHandler<StartCommand, Message>
{
    public async Task<Message> Handle(StartCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<User>(cancellationToken);

        var user = transaction.Set.AsNoTracking().FirstOrDefault(x => x.TelegramId == request.TelegramId);
        if (user == null)
        {
            user = new User
            {
                TelegramId = request.TelegramId,
                Username = request.Username,
            };
            transaction.Add(user);
            await transaction.CommitAsync(cancellationToken);
        }

        return new Message
        {
            Text = Messages.StartMessage
        };
    }
}