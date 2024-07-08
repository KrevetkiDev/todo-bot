namespace Krevetki.ToDoBot.Application.Common.Helpers;

public static class DateParser
{
    public static bool TryParseDate(string inputMessage, out DateOnly dateList)
    {
        var stringItems = inputMessage.Split(Messages.ListTasksByDateSignalSymbol);

        if (stringItems.Length == 2 && DateOnly.TryParse(stringItems[1], out var date))
        {
            dateList = date;
            return true;
        }

        dateList = default!;
        return false;
    }
}
