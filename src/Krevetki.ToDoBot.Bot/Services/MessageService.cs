using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Bot.Interfaces;

using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Krevetki.ToDoBot.Bot.Services;

public record MessageService : IMessageService
{
    public async Task SendMessageAsync(ITelegramBotClient client, Message message, long chatId, CancellationToken cancellationToken)
    {
        if (message.Keyboard != null && message.Keyboard.Buttons != null)
        {
            var keyboardMarkup = new InlineKeyboardMarkup(
                message.Keyboard.Buttons.Select(
                    row => row.Select(b => InlineKeyboardButton.WithCallbackData(b.Title, b.CallbackData))));

            await client.SendTextMessageAsync(
                chatId,
                message.Text,
                replyMarkup: keyboardMarkup,
                cancellationToken: cancellationToken);
        }
        else
        {
            await client.SendTextMessageAsync(chatId, message.Text, cancellationToken: cancellationToken);
        }
    }

    public async Task DeleteMessageAsync(ITelegramBotClient client, int messageId, long chatId, CancellationToken cancellationToken)
    {
        await client.DeleteMessageAsync(chatId, messageId, cancellationToken: cancellationToken);
        Console.WriteLine("Message deleted successfully."); //TODO: logger
    }
}
