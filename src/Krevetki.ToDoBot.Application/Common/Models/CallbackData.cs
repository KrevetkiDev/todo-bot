namespace Krevetki.ToDoBot.Application.Common.Models;

public class CallbackData
{
    public CallbackDataType CallbackType { get; set; }
}

public class CallbackData<T> : CallbackData
{
    public T Data { get; set; }
}
