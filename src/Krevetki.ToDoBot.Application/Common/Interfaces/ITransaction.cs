using Krevetki.ToDoBot.Domain.Entities.Base;

namespace Krevetki.ToDoBot.Application.Common.Interfaces;

public interface ITransaction<TEntity> : IDisposable, IAsyncDisposable
    where TEntity : EntityBase
{
    IQueryable<TEntity> Set { get; }

    void Commit();

    Task CommitAsync(CancellationToken cancellationToken);

    TEntity Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    TEntity Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);
}
