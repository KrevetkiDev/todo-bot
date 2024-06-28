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

            var keyboardIfMoreDayBeforeEvent =
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

            var keyboardIfMoreThreeHoursBeforeEvent =
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
                            }
                        ]
                    ]
                };

            var keyboardIfMoreOneHourBeforeEvent =
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
                            }
                        ]
                    ]
                };
            var now = DateTime.UtcNow;
            const int day = 24;
            const int threeHours = 3;
            const int oneHour = 1;

            if (todoItem.DateTimeToStart.Date != now.Date && todoItem.DateTimeToStart.Date == now.Date.AddDays(1))
            {
                messagesList.Add(
                    new Message
                    {
                        Text = Messages.AddTodoSuccessMessage(todoItem.Title, todoItem.DateTimeToStart),
                        Keyboard = keyboardIfMoreDayBeforeEvent
                    });
            }

            if (todoItem.DateTimeToStart.Date == now.Date.AddDays(1))
            {
                var restOfHoursInToday = day - now.Hour;
                var restOfHoursInTomorrow = todoItem.DateTimeToStart.Hour;
                var sum = restOfHoursInToday + restOfHoursInTomorrow;

                if (sum > threeHours && sum < day)
                {
                    messagesList.Add(
                        new Message
                        {
                            Text = Messages.AddTodoSuccessMessage(todoItem.Title, todoItem.DateTimeToStart),
                            Keyboard = keyboardIfMoreThreeHoursBeforeEvent
                        });
                }

                if (sum > oneHour && sum < threeHours)
                {
                    messagesList.Add(
                        new Message
                        {
                            Text = Messages.AddTodoSuccessMessage(todoItem.Title, todoItem.DateTimeToStart),
                            Keyboard = keyboardIfMoreOneHourBeforeEvent
                        });
                }

                if (sum < oneHour)
                {
                    messagesList.Add(
                        new Message
                        {
                            Text = Messages.AddTodoSuccessMessageIfLessThanHourBeforeEvent(todoItem.Title, todoItem.DateTimeToStart)
                        });
                }
            }

            var timeDifference = todoItem.DateTimeToStart - now;

            if (timeDifference.TotalHours > threeHours && timeDifference.TotalHours < day)
            {
                messagesList.Add(
                    new Message
                    {
                        Text = Messages.AddTodoSuccessMessage(todoItem.Title, todoItem.DateTimeToStart),
                        Keyboard = keyboardIfMoreThreeHoursBeforeEvent
                    });
            }

            if (timeDifference.TotalHours > oneHour && timeDifference.TotalHours < threeHours)
            {
                messagesList.Add(
                    new Message
                    {
                        Text = Messages.AddTodoSuccessMessage(todoItem.Title, todoItem.DateTimeToStart),
                        Keyboard = keyboardIfMoreOneHourBeforeEvent
                    });
            }

            if (timeDifference.TotalHours < oneHour)
            {
                messagesList.Add(
                    new Message
                    {
                        Text = Messages.AddTodoSuccessMessageIfLessThanHourBeforeEvent(todoItem.Title, todoItem.DateTimeToStart)
                    });
            }

            if (user == null)
            {
                messagesList.Add(new Message { Text = Messages.UserNotFoundMessage });
            }
        }

        return messagesList;
    }
}
