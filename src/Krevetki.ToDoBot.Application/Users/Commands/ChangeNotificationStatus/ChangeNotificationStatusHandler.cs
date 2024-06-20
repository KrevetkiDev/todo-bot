using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Domain.Entities;

using MediatR;

namespace Krevetki.ToDoBot.Application.Users.Commands.ChangeNotificationStatus
{
    public record ChangeNotificationStatusHandler(IRepository Repository) : IRequestHandler<ChangeNotificationStatusCommand>
    {
        public async Task Handle(ChangeNotificationStatusCommand request, CancellationToken cancellationToken)
        {
            // 1. проверить есть ли интервал
            // 1.1 если нету, то ничего не делаем
            // 1.2 если есть, то создаем уведомление

            // 1. создать объект уведомления
            // создать транзакцию по таблице дел -> вытащить из бд дело по id из команды
            // 2. присвоить объекту расчетное из интервала время
            // связать уведомление с делом
            // связать уведомление с юзером
            // сделать транзацию по таблице нотификейшенов
            // сохранить нотификейшн в бд

            if (request.TimeInterval.HasValue)
            {
                await using var transactionToDoItem = await Repository.BeginTransactionAsync<ToDoItem>(cancellationToken);
                var task = transactionToDoItem.Set.FirstOrDefault(x => x.Id == request.ToDoItemId);

                if (task != null)
                {
                    var newNotification = new Notification
                                          {
                                              NotificationTime = task.DateToStart
                                                                     .ToDateTime(task.TimeToStart)
                                                                     .AddHours((int)request.TimeInterval)
                                                                     .ToUniversalTime(),
                                              ToDoItemId = task.Id,
                                              UserId = task.UserId
                                          };

                    await using var transactionNotification = await Repository.BeginTransactionAsync<Notification>(cancellationToken);
                    transactionNotification.Add(newNotification);
                    await transactionNotification.CommitAsync(cancellationToken: cancellationToken);
                }
            }
        }
    }
}
