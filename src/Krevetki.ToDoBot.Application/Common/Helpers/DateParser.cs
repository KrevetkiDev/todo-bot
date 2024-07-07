using Krevetki.ToDoBot.Application.Common.Models;

namespace Krevetki.ToDoBot.Application.Common.Helpers;

public class DateParser
{
    public bool TryParseDate(string inputMessage, out DateDto dto)
    {
        var stringItems = inputMessage.Split('?');

        if (stringItems.Length == 2 && DateOnly.TryParse(stringItems[1], out var date))
        {
            dto = new DateDto() { Date = date };

            return true;
        }

        dto = default!;
        return false;
    }
}
