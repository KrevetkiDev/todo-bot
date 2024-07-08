using Krevetki.ToDoBot.Domain.Entities.Base;

namespace Krevetki.ToDoBot.Application.Common.Interfaces;

public interface IRepository
{
    ITransaction<TEntity> BeginTransaction<TEntity>()
        where TEntity : EntityBase;

    Task<ITransaction<TEntity>> BeginTransactionAsync<TEntity>(CancellationToken cancellationToken)
        where TEntity : EntityBase;
}
