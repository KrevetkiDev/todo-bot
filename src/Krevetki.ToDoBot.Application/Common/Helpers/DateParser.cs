namespace Krevetki.ToDoBot.Application.Common.Helpers;

public static class DateParser
{
    public static bool TryParseDate(string inputMessage, out DateOnly dateList)
    {
        dateList = default;
        return inputMessage.StartsWith(Messages.ListTasksByDateSignalSymbol)
               && DateOnly.TryParse(new string(inputMessage.Skip(1).ToArray()), out dateList);
    }
}
