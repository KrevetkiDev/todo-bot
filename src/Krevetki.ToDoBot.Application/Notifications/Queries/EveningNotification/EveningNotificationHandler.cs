using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.Commands.ChangeDateYesterdayToDoItems;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Application.Notifications.Queries.EveningNotification;

public record EveningNotificationHandler(IRepository Repository, IMessageService MessageService, ICallbackDataSaver CallbackDataSaver)
    : IRequestHandler<EveningNotificationQuery>
{
    public async Task Handle(EveningNotificationQuery request, CancellationToken cancellationToken)
    {
        var users = await GetUsersWithActiveEveningNotificationAsync(cancellationToken);

        foreach (var user in users)
        {
            var todoItems = await GetYesterdayToDoItemsAsync(user, cancellationToken);

            await NotifyUserAsync(user, todoItems, cancellationToken);
        }
    }

    private async Task<List<User>> GetUsersWithActiveEveningNotificationAsync(CancellationToken cancellationToken)
    {
        await using var transactionUser = await Repository.BeginTransactionAsync<User>(cancellationToken);
        return transactionUser.Set.AsNoTracking()
                              .Where(x => x.EveningNotificationStatus == Domain.Enums.EveningNotificationStatus.Active)
                              .ToList();
    }

    private async Task<InlineKeyboard> CreateKeyboardAsync(User user, ICollection<Guid> todoItemIds, CancellationToken cancellationToken)
    {
        var callbackData = new CallbackData<ChangeDateYesterdayToDoItemsCommand>
                           {
                               CallbackType = CallbackDataType.YesterdayTasksMoveToday,
                               Data = new() { User = user, ToDoItemIds = todoItemIds }
                           };

        var callbackDataString = (await CallbackDataSaver.SaveCallbackDataAsync(callbackData, cancellationToken)).ToString();

        return new InlineKeyboard
               {
                   Buttons =
                   [
                       [
                           new Button { Title = Common.Commands.DoneTaskCommand, CallbackData = callbackDataString }
                       ]
                   ]
               };
    }

    private async Task<List<ToDoItem>> GetYesterdayToDoItemsAsync(User user, CancellationToken cancellationToken)
    {
        await using var todoItemTransaction = await Repository.BeginTransactionAsync<ToDoItem>(cancellationToken);
        return todoItemTransaction.Set
                                  .Where(x => x.UserId == user.Id && x.DateTimeToStart.Date == DateTime.UtcNow.Date.AddDays(-1))
                                  .ToList();
    }

    private async Task NotifyUserAsync(User user, ICollection<ToDoItem> todoItems, CancellationToken cancellationToken)
    {
        if (todoItems.Any())
        {
            var tasksWithStatusDone = todoItems.Count(x => x.Status == ToDoItemStatus.Done);
            var tasksWithStatusNotToBeDone = todoItems.Count(x => x.Status == ToDoItemStatus.NotToBeDone);
            var tasksWithStatusNew = todoItems.Where(x => x.Status == ToDoItemStatus.New).ToList();

            var keyboard = await CreateKeyboardAsync(user, tasksWithStatusNew.Select(x => x.Id).ToArray(), cancellationToken);
            await MessageService.SendMessageAsync(
                new Message
                {
                    Text = Messages.EveningNotificationMessage(
                        tasksWithStatusDone.ToString(),
                        tasksWithStatusNotToBeDone.ToString(),
                        tasksWithStatusNew),
                    Keyboard = keyboard
                },
                user.ChatId,
                cancellationToken);
        }
        else
        {
            await MessageService.SendMessageAsync(
                new Message { Text = Messages.EveningNotificationNotDoneTasksYesterday },
                user.ChatId,
                cancellationToken);
        }
    }
}
