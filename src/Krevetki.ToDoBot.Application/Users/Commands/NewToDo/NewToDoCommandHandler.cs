using MediatR;

using ToDoBot.Application.Models.Models;
using ToDoBot.Domain;
using ToDoBot.Domain.Entities;

namespace ToDoBot.Application.Users.Commands.NewToDo;

public record NewToDoCommandHandler(IRepository Repository) : IRequestHandler<NewToDoCommand, Message>
{
    public async Task<Message> Handle(NewToDoCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await Repository.BeginTransactionAsync<User>(cancellationToken);

        var user = transaction.Set.FirstOrDefault(x => x.TelegramId == request.TelegramId);
        var todoItem = new ToDoItem()
                       {
                           Text = request.ToDoItemDto.Text,
                           DateToStart = request.ToDoItemDto.DateToStart,
                           TimeToStart = request.ToDoItemDto.TimeToStart
                       };

        user.Tasks.Add(todoItem);

        await transaction.CommitAsync(cancellationToken);

        return new Message { Text = Messages.AddTodoSuccessMessage(todoItem.Text, todoItem.DateToStart, todoItem.TimeToStart) };
    }
}
