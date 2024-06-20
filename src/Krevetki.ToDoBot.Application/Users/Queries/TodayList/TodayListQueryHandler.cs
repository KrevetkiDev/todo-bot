using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Domain.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Application.Users.Queries.TodayList;

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
                    Buttons =
                    [
                        [
                            new Button { Title = Common.Commands.DoneTaskCommand, CallbackData = $"Done {item.Id}" },
                            new Button { Title = Common.Commands.NotToBeDoneTaskCommand, CallbackData = $"NotToBeDone {item.Id}" }
                        ]
                    ]
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
