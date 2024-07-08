using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.Users.Queries.Help;

public class HelpTaskQuery : IRequest
{
    public User User { get; set; }
}
