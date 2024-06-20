namespace Krevetki.ToDoBot.Application.Common.Models;

public class CallbackData
{
    public CallbackDataType CallbackType { get; set; } // энум или строковые константы, что-то что сможет идентифицировать коллбек
}

public class CallbackData<T> : CallbackData
{
    public T Data { get; set; }
}
