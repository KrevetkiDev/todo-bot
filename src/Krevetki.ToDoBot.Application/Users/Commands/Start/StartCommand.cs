using MediatR;

using User = Krevetki.ToDoBot.Domain.Entities.User;

namespace Krevetki.ToDoBot.Application.Users.Commands.Start;

public class StartCommand : IRequest
{
    public User User { get; set; }
}
