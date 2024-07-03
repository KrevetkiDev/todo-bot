using MediatR;

using Message = Krevetki.ToDoBot.Application.Common.Models.Message;
using User = Krevetki.ToDoBot.Domain.Entities.User;

namespace Krevetki.ToDoBot.Application.Users.Commands.Start;

public class StartCommand : IRequest<Message>
{
    public User User { get; set; }
}
