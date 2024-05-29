namespace ToDoBot.Application;

public class Messages
{
    public const string StartMessage =
        "Приветик! Я твоя Напоминалочка. Можешь записывать в меня важные и не очень дела, а я обязательно все сохраню и напомню если попросишь. Могу записать твои планы на любую дату, а также хранить список дел, которые ты хочешь выполнять ежедневно.  Чтобы записать свое первое дело нажми на кнопку меню :)";

    public const string HelpMessage = "Для того чтобы записать новое дело отправь соощение в формате: \n!Помыть посуду, 27.10.2024, 17:30";

    public const string AddTodoErrorMessage = "Неправильный формат. Попробуй ещё раз";

    public static string AddTodoSuccessMessage(string task, DateOnly date, TimeOnly time) =>
        $"Дело: {task} . Запланировано на {date} в {time}";

    public const string StartNewTaskMessage = "!";
}
