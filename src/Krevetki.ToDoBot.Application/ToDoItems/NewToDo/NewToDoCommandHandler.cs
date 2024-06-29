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
        var messagesList = new List<Message>();

        await using var transaction = await Repository.BeginTransactionAsync<User>(cancellationToken);

        var user = transaction.Set.FirstOrDefault(x => x.TelegramId == request.TelegramId);

        if (user == null)
        {
            messagesList.Add(new Message { Text = Messages.UserNotFoundMessage });
            return messagesList;
        }

        var todoItem = new ToDoItem
                       {
                           Title = request.ToDoItemDto.Title, DateTimeToStart = request.ToDoItemDto.DateTimeToStart.ToUniversalTime()
                       };

        user.Tasks.Add(todoItem);

        await transaction.CommitAsync(cancellationToken);

        var keyboard = await GetKeyboard(todoItem, user, cancellationToken);

        messagesList.Add(
            new Message
            {
                Text = Messages.AddTodoSuccessMessageIfLessThanHourBeforeEvent(todoItem.Title, todoItem.DateTimeToStart),
                Keyboard = keyboard
            });

        return messagesList;
    }

    private async Task<InlineKeyboard> GetKeyboard(ToDoItem todoItem, User user, CancellationToken cancellationToken)
    {
        var keyboard = new InlineKeyboard();

        var hoursBeforeToBeDone = (todoItem.DateTimeToStart.ToUniversalTime() - DateTime.UtcNow).TotalHours;

        if (hoursBeforeToBeDone > (int)NotificationTimeIntervals.InHour)
        {
            keyboard.Buttons[0].Add(await GetDisableButton(todoItem, user, cancellationToken));
            keyboard.Buttons[0].Add(await GetHourButton(todoItem, user, cancellationToken));
        }

        if (hoursBeforeToBeDone > (int)NotificationTimeIntervals.InThreeHours)
        {
            keyboard.Buttons[0].Add(await GetThreeHoursButton(todoItem, user, cancellationToken));
        }

        if (hoursBeforeToBeDone > (int)NotificationTimeIntervals.InTwentyFourHours)
        {
            keyboard.Buttons[0].Add(await GetDayButton(todoItem, user, cancellationToken));
        }

        return keyboard;
    }

    private async Task<Button> GetDisableButton(ToDoItem todoItem, User user, CancellationToken cancellationToken)
    {
        var disableNotificationCallbackData =
            new CallbackData<ChangeNotificationStatusCommand>
            {
                Data = new() { ToDoItemId = todoItem.Id, TimeInterval = null, UserId = user.Id },
                CallbackType = CallbackDataType.NotificationInterval
            };

        return new Button
               {
                   Title = Common.Commands.DisableNotification,
                   CallbackData = (await CallbackDataSaver.SaveCallbackDataAsync(
                                       disableNotificationCallbackData,
                                       cancellationToken)).ToString(),
               };
    }

    private async Task<Button> GetHourButton(ToDoItem todoItem, User user, CancellationToken cancellationToken)
    {
        var inHourNotificationCallbackData =
            new CallbackData<ChangeNotificationStatusCommand>
            {
                Data = new() { ToDoItemId = todoItem.Id, TimeInterval = NotificationTimeIntervals.InHour, UserId = user.Id },
                CallbackType = CallbackDataType.NotificationInterval
            };

        return new Button
               {
                   Title = Common.Commands.NotificationInHour,
                   CallbackData = (await CallbackDataSaver.SaveCallbackDataAsync(
                                       inHourNotificationCallbackData,
                                       cancellationToken)).ToString()
               };
    }

    private async Task<Button> GetThreeHoursButton(ToDoItem todoItem, User user, CancellationToken cancellationToken)
    {
        var inHourNotificationCallbackData =
            new CallbackData<ChangeNotificationStatusCommand>
            {
                Data = new() { ToDoItemId = todoItem.Id, TimeInterval = NotificationTimeIntervals.InThreeHours, UserId = user.Id },
                CallbackType = CallbackDataType.NotificationInterval
            };

        return new Button
               {
                   Title = Common.Commands.NotificationInThreeHours,
                   CallbackData = (await CallbackDataSaver.SaveCallbackDataAsync(
                                       inHourNotificationCallbackData,
                                       cancellationToken)).ToString()
               };
    }

    private async Task<Button> GetDayButton(ToDoItem todoItem, User user, CancellationToken cancellationToken)
    {
        var inHourNotificationCallbackData =
            new CallbackData<ChangeNotificationStatusCommand>
            {
                Data = new() { ToDoItemId = todoItem.Id, TimeInterval = NotificationTimeIntervals.InTwentyFourHours, UserId = user.Id },
                CallbackType = CallbackDataType.NotificationInterval
            };

        return new Button
               {
                   Title = Common.Commands.NotificationInDay,
                   CallbackData = (await CallbackDataSaver.SaveCallbackDataAsync(
                                       inHourNotificationCallbackData,
                                       cancellationToken)).ToString()
               };
    }
}
