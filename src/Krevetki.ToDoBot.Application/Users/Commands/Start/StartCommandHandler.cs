using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Application.Users.Commands.Start;

public record StartCommandHandler(IRepository Repository) : IRequestHandler<StartCommand, Message>
{
    public async Task<Message> Handle(StartCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<User>(cancellationToken);

        var user = transaction.Set.AsNoTracking().FirstOrDefault(x => x.TelegramId == request.User.TelegramId);
        if (user == null)
        {
            user = new User { TelegramId = request.User.TelegramId, Username = request.User.Username, ChatId = request.User.ChatId };
            transaction.Add(user);
            await transaction.CommitAsync(cancellationToken);
        }

        return new Message { Text = Messages.StartMessage };
    }
}
