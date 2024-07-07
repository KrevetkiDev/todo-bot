using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.Notifications.Queries.EveningNotificationStatus;

public class EveningNotificationStatusQuery : IRequest
{
    public User User { get; set; }
}
