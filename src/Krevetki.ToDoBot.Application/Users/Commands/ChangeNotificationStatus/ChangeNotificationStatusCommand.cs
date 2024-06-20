using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

namespace Krevetki.ToDoBot.Application.Users.Commands.ChangeNotificationStatus;

public class ChangeNotificationStatusCommand : IRequest
{
    public Guid ToDoItemId { get; set; }

    public NotificationTimeIntervals? TimeInterval { get; set; }

    public Guid UserId { get; set; }
}
