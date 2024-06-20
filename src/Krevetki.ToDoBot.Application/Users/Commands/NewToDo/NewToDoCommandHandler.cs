using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.Users.Commands.ChangeNotificationStatus;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

using Newtonsoft.Json;

using CallbackData = Krevetki.ToDoBot.Domain.Entities.CallbackData;

namespace Krevetki.ToDoBot.Application.Users.Commands.NewToDo;

public record NewToDoCommandHandler(IRepository Repository) : IRequestHandler<NewToDoCommand, List<Message>>
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
                               Title = request.ToDoItemDto.Title,
                               DateToStart = request.ToDoItemDto.DateToStart,
                               TimeToStart = request.ToDoItemDto.TimeToStart
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
                                CallbackData = await SaveCallbackData(disableNotificationCallbackData, cancellationToken),
                            },
                            new Button
                            {
                                Title = Common.Commands.NotificationInHour,
                                CallbackData = await SaveCallbackData(inHourNotificationCallbackData, cancellationToken)
                            },
                            new Button
                            {
                                Title = Common.Commands.NotificationInThreeHours,
                                CallbackData = await SaveCallbackData(inThreeHoursNotificationCallbackData, cancellationToken)
                            },
                            new Button
                            {
                                Title = Common.Commands.NotificationInDay,
                                CallbackData = await SaveCallbackData(inTwentyFourNotificationCallbackData, cancellationToken)
                            },
                        ]
                    ]
                };

            messagesList.Add(
                new Message
                {
                    Text = Messages.AddTodoSuccessMessage(todoItem.Title, todoItem.DateToStart, todoItem.TimeToStart), Keyboard = keyboard
                });
        }

        if (user == null)
        {
            messagesList.Add(new Message { Text = Messages.UserNotFoundMessage });
        }

        return messagesList;
    }

    private async Task<string> SaveCallbackData(object data, CancellationToken cancellationToken)
    {
        await using var transactionCallbackData = await Repository.BeginTransactionAsync<CallbackData>(cancellationToken);
        var callbackData = new CallbackData { JsonData = JsonConvert.SerializeObject(data) };
        transactionCallbackData.Add(callbackData);
        await transactionCallbackData.CommitAsync(cancellationToken);
        return callbackData.Id.ToString();
    }
}
