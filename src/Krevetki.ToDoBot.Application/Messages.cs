using ToDoBot.Domain.Entities;

namespace ToDoBot.Application;

public class Messages
{
    public const string StartMessage =
        "Приветик! Я твоя Напоминалочка. Можешь записывать в меня важные и не очень дела, а я обязательно все сохраню и напомню если попросишь. Могу записать твои планы на любую дату, а также хранить список дел, которые ты хочешь выполнять ежедневно.  Чтобы записать свое первое дело нажми на кнопку меню :)";

    public const string HelpMessage = "Для того чтобы записать новое дело отправь соощение в формате: \n!Помыть посуду, 27.10.2024, 17:30";

    public const string AddTodoErrorMessage = "Неправильный формат. Попробуй ещё раз";

    public static string AddTodoSuccessMessage(string task, DateOnly date, TimeOnly time) =>
        $"Дело: {task} . Запланировано на {date} в {time}";

    public static string ToDoTaskToString(ToDoItem todoTask)
    {
        var time = todoTask.TimeToStart;
        var task = $"Дело: {todoTask.Title}. Запланировано на  {todoTask.DateToStart}";

        return task;
    }

    public const string StartNewTaskMessage = "!";

    public const string UserNotFoundMessage = "Пользователь не найден. Попробуй нажать команду старт";

    public const string NoTasksTodayMessage = "Сегодня нет дел";

    public static string CountTask(int countTasks) => $"Оставшихся задач на сегодня: {countTasks} ";
}
