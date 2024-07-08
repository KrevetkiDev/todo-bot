using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.ChangeToDoItemStatus;
using Krevetki.ToDoBot.Application.ToDoItems.TodayList;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Application.ToDoItems.Queries.ListTasksByDate;

public record ListTaskByDateHandler(IRepository Repository, ICallbackDataSaver CallbackDataSaver, IMessageService MessageService)
    : IRequestHandler<ListTaskByDateQuery>
{
    public async Task Handle(ListTaskByDateQuery request, CancellationToken cancellationToken)
    {
        await using var transactionToDoItem = await Repository.BeginTransactionAsync<ToDoItem>(cancellationToken);
        await using var transactionNotification = await Repository.BeginTransactionAsync<Notification>(cancellationToken);

        var tasksList = await transactionToDoItem.Set
                                                 .AsNoTracking()
                                                 .Where(
                                                     x => DateOnly.FromDateTime(x.DateTimeToStart.Date) == request.Date
                                                          && x.Status == ToDoItemStatus.New)
                                                 .ToListAsync(cancellationToken);

        if (tasksList.Count == 0)
        {
            await MessageService.SendMessageAsync(
                new Message { Text = Messages.NoTasksMessage },
                request.User.ChatId,
                cancellationToken);
            return;
        }

        foreach (var item in tasksList)
        {
            await MessageService.SendMessageAsync(
                await GetToDoItemMessage(item, transactionNotification, cancellationToken),
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

        var doneTaskCallbackData = new CallbackData<ChangeToDoItemStatusCommand>
                                   {
                                       CallbackType = CallbackDataType.TaskStatus,
                                       Data = new ChangeToDoItemStatusCommand { ToDoItemStatus = ToDoItemStatus.Done, ToDoItemId = item.Id }
                                   };

        var notDoneTaskCallbackData = new CallbackData<ChangeToDoItemStatusCommand>
                                      {
                                          CallbackType = CallbackDataType.TaskStatus,
                                          Data = new ChangeToDoItemStatusCommand
                                                 {
                                                     ToDoItemStatus = ToDoItemStatus.NotToBeDone, ToDoItemId = item.Id
                                                 }
                                      };

        var keyboard =
            new InlineKeyboard
            {
                Buttons =
                [
                    [
                        new Button
                        {
                            Title = ButtonsTittles.DoneTaskCommand,
                            CallbackData = (await CallbackDataSaver.SaveCallbackDataAsync(doneTaskCallbackData, cancellationToken))
                                .ToString()
                        },
                        new Button
                        {
                            Title = ButtonsTittles.NotToBeDoneTaskCommand,
                            CallbackData = (await CallbackDataSaver.SaveCallbackDataAsync(notDoneTaskCallbackData, cancellationToken))
                                .ToString()
                        }
                    ]
                ]
            };

        var messageText = Messages.ToDoTaskToString(item, notification == null);

        if (item.DateTimeToStart.Date >= DateTime.Today.Date)
        {
            return new Message { Text = messageText, Keyboard = keyboard };
        }

        return new Message { Text = messageText };
    }
}
