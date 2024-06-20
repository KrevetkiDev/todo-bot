using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Domain.Entities.Base;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Infrastructure.Persistence.Repository;

public record Repository(IDbContextFactory<DatabaseContext> DbContextFactory) : IRepository
{
    public ITransaction<TEntity> BeginTransaction<TEntity>()
        where TEntity : EntityBase =>
        new Transaction<TEntity>(DbContextFactory.CreateDbContext());

    public async Task<ITransaction<TEntity>> BeginTransactionAsync<TEntity>(CancellationToken cancellationToken)
        where TEntity : EntityBase =>
        new Transaction<TEntity>(await DbContextFactory.CreateDbContextAsync(cancellationToken));
}
