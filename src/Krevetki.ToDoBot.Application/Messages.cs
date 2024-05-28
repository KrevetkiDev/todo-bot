namespace ToDoBot.Application;

public class Messages
{
    public const string StartMessage =
        "Приветик! Я твоя Напоминалочка. Можешь записывать в меня важные и не очень дела, а я обязательно все сохраню и напомню если попросишь. Могу записать твои планы на любую дату, а также хранить список дел, которые ты хочешь выполнять ежедневно.  Чтобы записать свое первое дело нажми на кнопку меню :)";

    public const string HelpMessage = "Для того чтобы записать новое дело отправь соощение в формате: \n!Помыть посуду, 27.10.2024, 17:30";

    public const string AddTodoErrorMessage = "Неправильно введено время или дата. Попробуй ещё раз";

    public const string AddTodoSuccessMessage = "Дело: помыть посуду. Запланировано на 01.01.2024 в 9:00";

    public const string StartNewTaskMessage = "!";
}
