using MediatR;

using Microsoft.EntityFrameworkCore;

using ToDoBot.Application.Common.Models;
using ToDoBot.Application.Models.Models;
using ToDoBot.Domain.Entities;
using ToDoBot.Domain.Enums;

namespace ToDoBot.Application.Users.Queries;

public class TodayListQueryHandler : IRequestHandler<TodayListQuery, List<Message>>
{
    private readonly IRepository _repository;

    public TodayListQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Message>> Handle(TodayListQuery request, CancellationToken cancellationToken)
    {
        await using var transaction = await _repository.BeginTransactionAsync<ToDoItem>(cancellationToken);

        var todayTasksList = await transaction.Set
                                              .AsNoTracking()
                                              .Where(
                                                  x => x.DateToStart == DateOnly.FromDateTime(DateTime.Now)
                                                       && x.Status == ToDoItemStatus.New)
                                              .ToListAsync(cancellationToken);

        var messagesList = new List<Message>();
        foreach (var item in todayTasksList)
        {
            var keyboard =
                new InlineKeyboard
                {
                    Buttons = new List<List<Button>>
                              {
                                  new List<Button>
                                  {
                                      new Button { Title = Models.Commands.DoneTaskCommand, CallbackData = "Done" + " " + item.Id },
                                      new Button
                                      {
                                          Title = Models.Commands.NotToBeDoneTaskCommand, CallbackData = "NotToBeDone" + " " + item.Id
                                      }
                                  }
                              }
                };

            messagesList.Add(new Message { Text = Messages.ToDoTaskToString(item), Keyboard = keyboard });
        }

        if (messagesList.Count == 0)
        {
            messagesList.Add(new Message { Text = Messages.NoTasksTodayMessage });
        }

        return messagesList;
    }
}
