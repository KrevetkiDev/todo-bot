using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.Notifications.ChangeNotificationStatus;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

namespace Krevetki.ToDoBot.Application.ToDoItems.NewToDo;

public record NewToDoCommandHandler(IRepository Repository, ICallbackDataSaver CallbackDataSaver)
    : IRequestHandler<NewToDoCommand, List<Message>>
{
    public async Task<List<Message>> Handle(NewToDoCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<User>(cancellationToken);

        var user = transaction.Set.FirstOrDefault(x => x.TelegramId == request.TelegramId);

        var messagesList = new List<Message>();

        if (user != null)
        {
            var todoItem = new ToDoItem
                           {
                               Title = request.ToDoItemDto.Title, DateTimeToStart = request.ToDoItemDto.DateTimeToStart.ToUniversalTime()
                           };

            user.Tasks.Add(todoItem);

            await transaction.CommitAsync(cancellationToken);

            var disableNotificationCallbackData =
                new CallbackData<ChangeNotificationStatusCommand>
                {
                    Data = new() { ToDoItemId = todoItem.Id, TimeInterval = null, UserId = user.Id },
                    CallbackType = CallbackDataType.NotificationInterval
                };

            var inHourNotificationCallbackData =
                new CallbackData<ChangeNotificationStatusCommand>
                {
                    Data = new() { ToDoItemId = todoItem.Id, TimeInterval = NotificationTimeIntervals.InHour, UserId = user.Id },
                    CallbackType = CallbackDataType.NotificationInterval
                };

            var inThreeHoursNotificationCallbackData =
                new CallbackData<ChangeNotificationStatusCommand>
                {
                    Data = new() { ToDoItemId = todoItem.Id, TimeInterval = NotificationTimeIntervals.InThreeHours, UserId = user.Id },
                    CallbackType = CallbackDataType.NotificationInterval
                };

            var inTwentyFourNotificationCallbackData =
                new CallbackData<ChangeNotificationStatusCommand>
                {
                    Data = new() { ToDoItemId = todoItem.Id, TimeInterval = NotificationTimeIntervals.InTwentyFourHours, UserId = user.Id },
                    CallbackType = CallbackDataType.NotificationInterval
                };

            var keyboard =
                new InlineKeyboard
                {
                    Buttons =
                    [
                        [
                            new Button
                            {
                                Title = Common.Commands.DisableNotification,
                                CallbackData = (await CallbackDataSaver.SaveCallbackDataMethod(
                                                    disableNotificationCallbackData,
                                                    cancellationToken)).ToString(),
                            },
                            new Button
                            {
                                Title = Common.Commands.NotificationInHour,
                                CallbackData = (await CallbackDataSaver.SaveCallbackDataMethod(
                                                    inHourNotificationCallbackData,
                                                    cancellationToken)).ToString()
                            },
                            new Button
                            {
                                Title = Common.Commands.NotificationInThreeHours,
                                CallbackData = (await CallbackDataSaver.SaveCallbackDataMethod(
                                                    inThreeHoursNotificationCallbackData,
                                                    cancellationToken)).ToString()
                            },
                            new Button
                            {
                                Title = Common.Commands.NotificationInDay,
                                CallbackData = (await CallbackDataSaver.SaveCallbackDataMethod(
                                                    inTwentyFourNotificationCallbackData,
                                                    cancellationToken)).ToString()
                            },
                        ]
                    ]
                };

            messagesList.Add(
                new Message { Text = Messages.AddTodoSuccessMessage(todoItem.Title, todoItem.DateTimeToStart), Keyboard = keyboard });
        }

        if (user == null)
        {
            messagesList.Add(new Message { Text = Messages.UserNotFoundMessage });
        }

        return messagesList;
    }
}
