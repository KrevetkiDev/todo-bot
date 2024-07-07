using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.Notifications.Queries.EveningNotification;

public class EveningNotificationQuery : IRequest
{
    public User User { get; set; }
}
