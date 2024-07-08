using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.Commands.ChangeEveningNotification;

public class ChangeEveningNotificationStatusCommand : IRequest
{
    public User User { get; set; }

    public EveningNotificationStatus EveningNotificationStatus { get; set; }
}
