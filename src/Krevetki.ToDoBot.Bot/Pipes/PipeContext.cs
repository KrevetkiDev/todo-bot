
using ToDoBot.Application.Models.Models;

namespace TodoBot.Bot.Pipes;

public class PipeContext
{
    /// <summary>
    /// Id пользователя приславшего сообщение
    /// </summary>
    public long TelegramId { get; set; }

    /// <summary>
    /// Юзернейм приславшего сообщение
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Message { get; set; } = default!;

    /// <summary>
    /// Коллекция для ответных сообщений
    /// </summary>
    public ICollection<Message> ResponseMessages { get; set; } = new List<Message>();
}