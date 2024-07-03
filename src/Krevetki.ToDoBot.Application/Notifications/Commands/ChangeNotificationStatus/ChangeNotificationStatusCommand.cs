using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

namespace Krevetki.ToDoBot.Application.Notifications.ChangeNotificationStatus;

public class ChangeNotificationStatusCommand : IRequest<Message>, IRequest
{
    public Guid ToDoItemId { get; set; }

    public NotificationTimeIntervals? TimeInterval { get; set; }

    public User User { get; set; }
}
