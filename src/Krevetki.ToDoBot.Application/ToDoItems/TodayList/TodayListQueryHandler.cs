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
        await using var transaction = await Repository.BeginTransactionAsync<ToDoItem>(cancellationToken);

        var todayTasksList = await transaction.Set
                                              .AsNoTracking()
                                              .Where(
                                                  x => x.DateTimeToStart.Date == DateTime.Today.ToUniversalTime().Date
                                                       && x.Status == ToDoItemStatus.New)
                                              .ToListAsync(cancellationToken);

        var messagesList = new List<Message>();

        foreach (var item in todayTasksList)
        {
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

            messagesList.Add(new Message { Text = Messages.ToDoTaskToString(item), Keyboard = keyboard });
        }

        if (messagesList.Count == 0)
        {
            messagesList.Add(new Message { Text = Messages.NoTasksTodayMessage });
        }

        return messagesList;
    }
}
