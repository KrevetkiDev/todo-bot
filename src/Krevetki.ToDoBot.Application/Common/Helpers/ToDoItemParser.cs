using Krevetki.ToDoBot.Application.Common.Models;

namespace Krevetki.ToDoBot.Application.Common.Helpers;

public class ToDoItemParser
{
    public bool TryParseToDoItem(string inputMessage, out ToDoItemDto dto)
    {
        var stringItems = inputMessage.Split('!', ',');

        if (stringItems.Length == 3 && DateTime.TryParse(stringItems[2], out var dateTime))
        {
            dto = new ToDoItemDto { Title = stringItems[1], DateTimeToStart = dateTime };

            return true;
        }

        dto = default!;
        return false;
    }
}
