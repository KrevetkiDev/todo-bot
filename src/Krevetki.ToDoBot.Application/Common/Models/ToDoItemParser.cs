using ToDoBot.Application.Models.Models;

namespace ToDoBot.Application.Common.Models;

public class ToDoItemParser
{
    public bool TryParse(string inputMessage, out ToDoItemDto dto)
    {
        var toDoItemDto = new ToDoItemDto();

        var stringItems = inputMessage.Split('!', ',').ToList();

        if (stringItems.Count >= 3)
        {
            toDoItemDto.Text = stringItems[1];

            if (DateTime.TryParse(stringItems[2] + " " + stringItems[3], out DateTime dateTime))
            {
                toDoItemDto.DateToStart = DateOnly.FromDateTime(dateTime);
                toDoItemDto.TimeToStart = TimeOnly.FromDateTime(dateTime);

                dto = toDoItemDto;
                return true;
            }
        }

        dto = null;
        return false;
    }
}
