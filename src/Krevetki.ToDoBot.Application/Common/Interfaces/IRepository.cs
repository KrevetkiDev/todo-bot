using ToDoBot.Domain.Entities;

namespace ToDoBot.Application;

public interface IRepository
{
    ITransaction<TEntity> BeginTransaction<TEntity>()
        where TEntity : EntityBase;

    Task<ITransaction<TEntity>> BeginTransactionAsync<TEntity>(CancellationToken cancellationToken) where TEntity : EntityBase;
}