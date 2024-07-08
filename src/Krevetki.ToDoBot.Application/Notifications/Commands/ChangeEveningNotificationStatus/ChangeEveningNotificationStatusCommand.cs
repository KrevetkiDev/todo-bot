using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

namespace Krevetki.ToDoBot.Application.Notifications.Commands.ChangeEveningNotificationStatus;

public class ChangeEveningNotificationStatusCommand : UserRequest, IRequest
{
    public EveningNotificationStatus EveningNotificationStatus { get; set; }
}
