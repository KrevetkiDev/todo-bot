using Krevetki.ToDoBot.Domain.Entities;

namespace Krevetki.ToDoBot.Application;

public class Messages
{
    public const string StartMessage =
        "Приветик! Я твоя Напоминалочка. Можешь записывать в меня важные и не очень дела, а я обязательно все сохраню и напомню если попросишь. Могу записать твои планы на любую дату, а также хранить список дел, которые ты хочешь выполнять ежедневно.  Чтобы записать свое первое дело нажми на кнопку меню :)";

    public const string HelpMessage = "Для того чтобы записать новое дело отправь соощение в формате: \n!Помыть посуду, 27.10.2024, 17:30";

    public const string AddTodoErrorMessage = "Неправильный формат. Попробуй ещё раз";

    public static string AddTodoSuccessMessage(string task, DateTime dateTimeToStart) =>
        $"Дело: {task} . Запланировано на {dateTimeToStart.ToLocalTime()}. Напомнить?";

    public static string ToDoTaskToString(ToDoItem todoTask)
    {
        var task = $"Дело: {todoTask.Title}. Запланировано на  {todoTask.DateTimeToStart.ToLocalTime()}";

        return task;
    }

    public const string StartNewTaskMessage = "!";

    public const string UserNotFoundMessage = "Пользователь не найден. Попробуй нажать команду старт";

    public const string NoTasksTodayMessage = "Сегодня нет дел";

    public static string NotificationMessage(ToDoItem toDoItem) =>
        $"Напоминаю! Дело: {toDoItem.Title} запланировано в {toDoItem.DateTimeToStart.ToLocalTime()}";

    public static string CountTask(int countTasks) => $"Оставшихся задач на сегодня: {countTasks} ";

    public const string NotificationOn = "Хорошо, обязательно напомню!";

    public const string NotificationDisable = "Хорошо, не буду напоминать!";

    public const string NotificationAlreadyExist = "Уведомление уже поставлено!";

    public static string AddTodoSuccessMessageIfLessThanHourBeforeEvent(string task, DateTime dateTimeToStart) =>
        $"Дело: {task} . Запланировано на {dateTimeToStart.ToLocalTime()}.";
}
