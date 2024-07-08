using System.Text;

using Krevetki.ToDoBot.Application.Common.Models;
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

    public static string ToDoTaskToString(ToDoItem todoTask, bool isNotificationExists)
    {
        string task;
        if (!isNotificationExists)
        {
            task =
                $"Дело: {todoTask.Title}. Запланировано на  {todoTask.DateTimeToStart.ToLocalTime()}. {ButtonsTittles.NotificationsActive}";
        }
        else
        {
            task =
                $"Дело: {todoTask.Title}. Запланировано на  {todoTask.DateTimeToStart.ToLocalTime()}. {ButtonsTittles.NotificationNotActive}";
        }

        return task;
    }

    public const string StartNewTaskSygnalSymbol = "!";

    public const string ListTasksByDateSignalSymbol = "?";

    public const string UserNotFoundMessage = "Пользователь не найден. Попробуй нажать команду старт";

    public const string NoTasksMessage = "Дел не осталось";

    public static string NotificationMessage(ToDoItem toDoItem) =>
        $"Напоминаю! Дело: {toDoItem.Title} запланировано в {toDoItem.DateTimeToStart.ToLocalTime()}";

    public static string CountTask(int countTasks) => $"Оставшихся задач на сегодня: {countTasks - 1} ";

    public const string NotificationOn = "Хорошо, обязательно напомню!";

    public const string NotificationDisable = "Хорошо, не буду напоминать!";

    public const string NotificationAlreadyExist = "Уведомление уже поставлено!";

    public static string AddTodoSuccessMessageIfLessThanHourBeforeEvent(string task, DateTime dateTimeToStart) =>
        $"Дело: {task} . Запланировано на {dateTimeToStart.ToLocalTime()}.";

    public static string EveningNotificationStatusMessage =>
        "Дорогой пользователь! Каждый день в 00:00 я буду присылать тебе итог прошедшего дня. Ты можешь выключить/включить эту функцию. С любовью, твоя Напоминалочка:*";

    public static string EveningNotificationMessage(
        string countTasksWithStatusDone,
        string countTasksWithStatusNotToBeDone,
        List<ToDoItem> newTasksList)
    {
        var toDoList = new List<string>();

        foreach (var task in newTasksList)
        {
            toDoList.Add($"{task.Title} {task.DateTimeToStart}.\n");
        }

        var list = new StringBuilder();
        list.Append(
            $"Отправляю итог сегодняшнего дня. Выполненных дел: {countTasksWithStatusDone}. Невыполненных дел: {countTasksWithStatusNotToBeDone}. ");
        list.Append("Оставшиеся дела: \n");
        list.Append(string.Join(", \n", toDoList));
        list.Append(" Перенести их на завтра?");

        return list.ToString();
    }

    public const string EveningNotificationActive = "Хорошо, буду присылать тебе итог дня. Ровно в полночь!";

    public const string EveningNotificationNotDisable = "Хорошо, никаких итогов дня, спи спокойно!";

    public const string EveningNotificationNotDoneTasksYesterday = "Вчера ты не выполнил ни одного дела.";
}
