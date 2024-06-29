using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;

using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Krevetki.ToDoBot.Infrastructure.Services;

public record MessageService(ITelegramClientHolder TelegramClientHolder) : IMessageService
{
    public async Task SendMessageAsync(Message message, long chatId, CancellationToken cancellationToken)
    {
        if (message.Keyboard != null && message.Keyboard.Buttons != null)
        {
            var keyboardMarkup = new InlineKeyboardMarkup(
                message.Keyboard.Buttons.Select(
                    row => row.Select(b => InlineKeyboardButton.WithCallbackData(b.Title, b.CallbackData))));

            await TelegramClientHolder.Client.SendTextMessageAsync(
                chatId,
                message.Text,
                replyMarkup: keyboardMarkup,
                cancellationToken: cancellationToken);
        }
        else
        {
            await TelegramClientHolder.Client.SendTextMessageAsync(chatId, message.Text, cancellationToken: cancellationToken);
        }
    }

    public async Task DeleteMessageAsync(int messageId, long chatId, CancellationToken cancellationToken)
    {
        await TelegramClientHolder.Client.DeleteMessageAsync(chatId, messageId, cancellationToken: cancellationToken);
    }
}
