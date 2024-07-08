using Telegram.Bot;

namespace Krevetki.ToDoBot.Application.Common.Interfaces;

public interface ITelegramClientHolder
{
    ITelegramBotClient Client { get; }
}
