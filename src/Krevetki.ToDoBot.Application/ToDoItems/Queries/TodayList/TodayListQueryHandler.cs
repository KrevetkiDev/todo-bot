using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.ChangeToDoItemStatus;
using Krevetki.ToDoBot.Application.ToDoItems.TodayList;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Application.ToDoItems.Queries.TodayList;

public record TodayListQueryHandler(IRepository Repository, ICallbackDataSaver CallbackDataSaver, IMessageService MessageService)
    : IRequestHandler<TodayListQuery>
{
    public async Task Handle(TodayListQuery request, CancellationToken cancellationToken)
    {
        await using var transactionToDoItem = await Repository.BeginTransactionAsync<ToDoItem>(cancellationToken);
        await using var transactionNotification = await Repository.BeginTransactionAsync<Notification>(cancellationToken);

        var todayTasksList = await transactionToDoItem.Set
                                                      .AsNoTracking()
                                                      .Where(
                                                          x => x.DateTimeToStart.Date == DateTime.Today.ToUniversalTime().Date
                                                               && x.Status == ToDoItemStatus.New)
                                                      .ToListAsync(cancellationToken);

        foreach (var item in todayTasksList)
        {
            await MessageService.SendMessageAsync(
                await GetToDoItemMessage(item, transactionNotification, cancellationToken),
                request.User.ChatId,
                cancellationToken);
        }

        if (todayTasksList.Count == 0)
        {
            await MessageService.SendMessageAsync(
                new Message { Text = Messages.NoTasksTodayMessage },
                request.User.ChatId,
                cancellationToken);
        }
    }

    private async Task<Message> GetToDoItemMessage(
        ToDoItem item,
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
                            CallbackData = (await CallbackDataSaver.SaveCallbackDataAsync(doneTaskCallbackData, cancellationToken))
                                .ToString()
                        },
                        new Button
                        {
                            Title = Common.Commands.NotToBeDoneTaskCommand,
                            CallbackData = (await CallbackDataSaver.SaveCallbackDataAsync(notDoneTaskCallbackData, cancellationToken))
                                .ToString()
                        }
                    ]
                ]
            };

        return new Message { Text = Messages.ToDoTaskToString(item, notification == null), Keyboard = keyboard };
    }
}
