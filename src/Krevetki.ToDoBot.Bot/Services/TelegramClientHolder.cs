using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Infrastructure.Options;

using Microsoft.Extensions.Options;

using Telegram.Bot;

namespace Krevetki.ToDoBot.Bot.Services;

public class TelegramClientHolder(IOptions<TelegramOptions> options) : ITelegramClientHolder
{
    public ITelegramBotClient Client { get; } = new TelegramBotClient(options.Value.Token);
}
