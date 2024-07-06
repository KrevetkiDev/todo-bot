using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.Notifications.ChangeNotificationStatus;
using Krevetki.ToDoBot.Application.ToDoItems.NewToDo;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

using Message = Krevetki.ToDoBot.Application.Common.Models.Message;

namespace Krevetki.ToDoBot.Application.ToDoItems.Commands.NewToDo;

public record NewToDoCommandHandler(IRepository Repository, ICallbackDataSaver CallbackDataSaver, IMessageService MessageService)
    : IRequestHandler<NewToDoCommand>
{
    public async Task Handle(NewToDoCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<ToDoItem>(cancellationToken);

        if (request.User == null)
        {
            await MessageService.SendMessageAsync(
                new Message { Text = Messages.UserNotFoundMessage },
                request.User.ChatId,
                cancellationToken);
            return;
        }

        var todoItem = new ToDoItem
                       {
                           Title = request.ToDoItemDto.Title,
                           DateTimeToStart = request.ToDoItemDto.DateTimeToStart.ToUniversalTime(),
                           UserId = request.User.Id
                       };

        transaction.Add(todoItem);

        await transaction.CommitAsync(cancellationToken);

        var keyboard = await GetKeyboard(todoItem, cancellationToken);

        await MessageService.SendMessageAsync(
            new Message
            {
                Text = Messages.AddTodoSuccessMessageIfLessThanHourBeforeEvent(todoItem.Title, todoItem.DateTimeToStart),
                Keyboard = keyboard
            },
            request.User.ChatId,
            cancellationToken);
    }

    private async Task<InlineKeyboard> GetKeyboard(ToDoItem todoItem, CancellationToken cancellationToken)
    {
        var keyboard = new InlineKeyboard();

        var hoursBeforeToBeDone = (todoItem.DateTimeToStart.ToUniversalTime() - DateTime.UtcNow).TotalHours;

        if (hoursBeforeToBeDone > (int)NotificationTimeIntervals.InHour)
        {
            keyboard.Buttons[0].Add(await GetDisableButton(todoItem, cancellationToken));
            keyboard.Buttons[0].Add(await GetHourButton(todoItem, cancellationToken));
        }

        if (hoursBeforeToBeDone > (int)NotificationTimeIntervals.InThreeHours)
        {
            keyboard.Buttons[0].Add(await GetThreeHoursButton(todoItem, cancellationToken));
        }

        if (hoursBeforeToBeDone > (int)NotificationTimeIntervals.InTwentyFourHours)
        {
            keyboard.Buttons[0].Add(await GetDayButton(todoItem, cancellationToken));
        }

        return keyboard;
    }

    private async Task<Button> GetDisableButton(ToDoItem todoItem, CancellationToken cancellationToken)
    {
        var disableNotificationCallbackData =
            new CallbackData<ChangeNotificationStatusCommand>
            {
                Data = new() { ToDoItemId = todoItem.Id, TimeInterval = null }, CallbackType = CallbackDataType.NotificationInterval
            };

        return new Button
               {
                   Title = Common.Commands.DisableNotification,
                   CallbackData = (await CallbackDataSaver.SaveCallbackDataAsync(
                                       disableNotificationCallbackData,
                                       cancellationToken)).ToString(),
               };
    }

    private async Task<Button> GetHourButton(ToDoItem todoItem, CancellationToken cancellationToken)
    {
        var inHourNotificationCallbackData =
            new CallbackData<ChangeNotificationStatusCommand>
            {
                Data = new() { ToDoItemId = todoItem.Id, TimeInterval = NotificationTimeIntervals.InHour, },
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

    private async Task<Button> GetThreeHoursButton(ToDoItem todoItem, CancellationToken cancellationToken)
    {
        var inHourNotificationCallbackData =
            new CallbackData<ChangeNotificationStatusCommand>
            {
                Data = new() { ToDoItemId = todoItem.Id, TimeInterval = NotificationTimeIntervals.InThreeHours },
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

    private async Task<Button> GetDayButton(ToDoItem todoItem, CancellationToken cancellationToken)
    {
        var inHourNotificationCallbackData =
            new CallbackData<ChangeNotificationStatusCommand>
            {
                Data = new() { ToDoItemId = todoItem.Id, TimeInterval = NotificationTimeIntervals.InTwentyFourHours },
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
