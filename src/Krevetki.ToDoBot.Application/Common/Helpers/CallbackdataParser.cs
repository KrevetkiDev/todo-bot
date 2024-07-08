using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Enums;

namespace Krevetki.ToDoBot.Application.Common.Helpers;

public class CallbackdataParser
{
    public static bool TryParseCallbackdata(string callbackData, out CallbackDataDto dto)
    {
        var stringItems = callbackData.Split(' ');

        if (stringItems.Length == 2
            && Enum.TryParse<ToDoItemStatus>(stringItems[0], out var status)
            && Guid.TryParse(stringItems[1], out var id))
        {
            dto = new CallbackDataDto { Status = status, ToDoItemId = id, CallbackType = status.ToString() };
            return true;
        }

        dto = default!;
        return false;
    }
}
