using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

namespace Krevetki.ToDoBot.Application.Notifications.Commands.ChangeNotificationStatus;

public class ChangeNotificationStatusCommand : UserRequest, IRequest
{
    public Guid ToDoItemId { get; set; }

    public NotificationTimeIntervals? TimeInterval { get; set; }
}
