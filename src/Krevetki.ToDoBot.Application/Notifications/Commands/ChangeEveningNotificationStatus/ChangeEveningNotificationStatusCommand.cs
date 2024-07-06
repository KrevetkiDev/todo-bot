using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.Commands.ChangeEveningNotification;

public class ChangeEveningNotificationStatusCommand : IRequest
{
    public User User { get; set; }

    public Domain.Enums.EveningNotificationStatus EveningNotificationStatus { get; set; }
}
