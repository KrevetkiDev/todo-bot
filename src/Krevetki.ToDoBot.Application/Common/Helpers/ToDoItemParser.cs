using ToDoBot.Application.Models.Models;

namespace ToDoBot.Application.Common.Models;

public class ToDoItemParser
{
    public bool TryParseToDoItem(string inputMessage, out ToDoItemDto dto)
    {
        var stringItems = inputMessage.Split('!', ',');

        if (stringItems.Length >= 3 && DateOnly.TryParse(stringItems[2], out var date) && TimeOnly.TryParse(stringItems[3], out var time))
        {
            dto = new ToDoItemDto { Title = stringItems[1], DateToStart = date, TimeToStart = time };

            return true;
        }

        dto = default!;
        return false;
    }
}
