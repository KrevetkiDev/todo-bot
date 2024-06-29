using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.ChangeToDoItemStatus;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Application.ToDoItems.TodayList;

public record TodayListQueryHandler(IRepository Repository, ICallbackDataSaver CallbackDataSaver)
    : IRequestHandler<TodayListQuery, List<Message>>
{
    public async Task<List<Message>> Handle(TodayListQuery request, CancellationToken cancellationToken)
    {
        await using var transactionToDoItem = await Repository.BeginTransactionAsync<ToDoItem>(cancellationToken);
        await using var transactionNotification = await Repository.BeginTransactionAsync<Notification>(cancellationToken);
        await using var transactionUser = await Repository.BeginTransactionAsync<User>(cancellationToken);

        var user = transactionUser.Set.FirstOrDefault(x => x.TelegramId == request.TelegramId)!;

        var todayTasksList = await transactionToDoItem.Set
                                                      .AsNoTracking()
                                                      .Where(
                                                          x => x.DateTimeToStart.Date == DateTime.Today.ToUniversalTime().Date
                                                               && x.Status == ToDoItemStatus.New)
                                                      .ToListAsync(cancellationToken);

        var messagesList = new List<Message>();

        foreach (var item in todayTasksList)
        {
            messagesList.Add(await GetToDoItemMessage(item, user, transactionNotification, cancellationToken));
        }

        if (messagesList.Count == 0)
        {
            messagesList.Add(new Message { Text = Messages.NoTasksTodayMessage });
        }

        return messagesList;
    }

    private async Task<Message> GetToDoItemMessage(
        ToDoItem item,
        User user,
        ITransaction<Notification> transaction,
        CancellationToken cancellationToken)
    {
        var notification = transaction.Set.FirstOrDefault(x => x.ToDoItem.Id == item.Id);

        var doneTaskCallbackData =
            new CallbackData<ChangeToDoItemStatusCommand>
            {
                CallbackType = CallbackDataType.TaskStatus, Data = new() { ToDoItemStatus = ToDoItemStatus.Done, ToDoItemId = item.Id }
            };

        var notDoneTaskCallbackData = new CallbackData<ChangeToDoItemStatusCommand>
                                      {
                                          CallbackType = CallbackDataType.TaskStatus,
                                          Data = new() { ToDoItemStatus = ToDoItemStatus.NotToBeDone, ToDoItemId = item.Id }
                                      };

        var keyboard =
            new InlineKeyboard
            {
                Buttons =
                [
                    [
                        new Button
                        {
                            Title = Common.Commands.DoneTaskCommand,
                            CallbackData = (await CallbackDataSaver.SaveCallbackDataMethod(doneTaskCallbackData, cancellationToken))
                                .ToString()
                        },
                        new Button
                        {
                            Title = Common.Commands.NotToBeDoneTaskCommand,
                            CallbackData = (await CallbackDataSaver.SaveCallbackDataMethod(notDoneTaskCallbackData, cancellationToken))
                                .ToString()
                        }
                    ]
                ]
            };

        return new Message { Text = Messages.ToDoTaskToString(item, notification == null), Keyboard = keyboard };
    }
}
