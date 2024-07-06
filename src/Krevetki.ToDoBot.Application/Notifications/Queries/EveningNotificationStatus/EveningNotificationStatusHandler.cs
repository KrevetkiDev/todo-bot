using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Application.ToDoItems.Commands.ChangeEveningNotification;

using MediatR;

namespace Krevetki.ToDoBot.Application.Notifications.Queries.EveningNotificationStatus;

public record EveningNotificationStatusHandler(IRepository Repository, ICallbackDataSaver CallbackDataSaver, IMessageService MessageService)
    : IRequestHandler<EveningNotificationStatusQuery>
{
    public async Task Handle(EveningNotificationStatusQuery request, CancellationToken cancellationToken)
    {
        var disableEveningNotificationCallbackData =
            new CallbackData<ChangeEveningNotificationStatusCommand>
            {
                CallbackType = CallbackDataType.EveningNotificationStatus,
                Data = new() { EveningNotificationStatus = Domain.Enums.EveningNotificationStatus.Disable }
            };

        var activeEveningNotificationCallbackData =
            new CallbackData<ChangeEveningNotificationStatusCommand>
            {
                CallbackType = CallbackDataType.EveningNotificationStatus,
                Data = new() { EveningNotificationStatus = Domain.Enums.EveningNotificationStatus.Active }
            };

        var keyboard =
            new InlineKeyboard
            {
                Buttons =
                [
                    [
                        new Button
                        {
                            Title = Common.Commands.NotificationsActive,
                            CallbackData = (await CallbackDataSaver.SaveCallbackDataAsync(
                                                activeEveningNotificationCallbackData,
                                                cancellationToken))
                                .ToString()
                        },
                        new Button
                        {
                            Title = Common.Commands.NotificationNotActive,
                            CallbackData = (await CallbackDataSaver.SaveCallbackDataAsync(
                                                disableEveningNotificationCallbackData,
                                                cancellationToken))
                                .ToString()
                        }
                    ]
                ]
            };

        await MessageService.SendMessageAsync(
            new Message { Text = Messages.EveningNotificationStatusMessage, Keyboard = keyboard },
            request.User.ChatId,
            cancellationToken);
    }
}
