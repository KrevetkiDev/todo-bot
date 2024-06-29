using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.Notifications.ChangeNotificationStatus
{
    public record ChangeNotificationStatusHandler(IRepository Repository) : IRequestHandler<ChangeNotificationStatusCommand, Message>
    {
        public async Task<Message> Handle(ChangeNotificationStatusCommand request, CancellationToken cancellationToken)
        {
            await using var transactionNotification = await Repository.BeginTransactionAsync<Notification>(cancellationToken);
            var notification = transactionNotification.Set.FirstOrDefault(x => x.ToDoItemId == request.ToDoItemId);

            if ((request.TimeInterval != null)
                && notification == null)
            {
                await using var transactionToDoItem = await Repository.BeginTransactionAsync<ToDoItem>(cancellationToken);
                var task = transactionToDoItem.Set.FirstOrDefault(x => x.Id == request.ToDoItemId);

                if (task != null)
                {
                    var newNotification = new Notification
                                          {
                                              NotificationTime =
                                                  task.DateTimeToStart.ToUniversalTime().AddHours(-(int)request.TimeInterval),
                                              ToDoItemId = task.Id,
                                              UserId = task.UserId
                                          };

                    transactionNotification.Add(newNotification);
                    await transactionNotification.CommitAsync(cancellationToken: cancellationToken);

                    return new Message() { Text = Messages.NotificationOn };
                }
            }

            if (request.TimeInterval == null && notification == null)
            {
                return new Message() { Text = Messages.NotificationDisable };
            }

            return new Message() { Text = Messages.NotificationAlreadyExist };
        }
    }
}
