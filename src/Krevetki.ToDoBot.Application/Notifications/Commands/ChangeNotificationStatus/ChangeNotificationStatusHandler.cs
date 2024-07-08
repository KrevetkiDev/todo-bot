using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.Notifications.ChangeNotificationStatus;
using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.Notifications.Commands.ChangeNotificationStatus
{
    public record ChangeNotificationStatusHandler(IRepository Repository, IMessageService MessageService)
        : IRequestHandler<ChangeNotificationStatusCommand>
    {
        public async Task Handle(ChangeNotificationStatusCommand request, CancellationToken cancellationToken)
        {
            await using var transactionNotification = await Repository.BeginTransactionAsync<Notification>(cancellationToken);
            var notification = transactionNotification.Set.FirstOrDefault(x => x.ToDoItemId == request.ToDoItemId);

            if (request.TimeInterval != null
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

                    await MessageService.SendMessageAsync(
                        new Message() { Text = Messages.NotificationOn },
                        request.User.ChatId,
                        cancellationToken);
                }
            }

            if (request.TimeInterval == null && notification == null)
            {
                await MessageService.SendMessageAsync(
                    new Message() { Text = Messages.NotificationDisable },
                    request.User.ChatId,
                    cancellationToken);
            }

            if (notification != null)
            {
                await MessageService.SendMessageAsync(
                    new Message() { Text = Messages.NotificationAlreadyExist },
                    request.User.ChatId,
                    cancellationToken);
            }
        }
    }
}
