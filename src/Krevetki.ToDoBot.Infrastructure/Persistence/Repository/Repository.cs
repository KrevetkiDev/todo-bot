using Microsoft.EntityFrameworkCore;
using ToDoBot.Application;
using ToDoBot.Domain.Entities;

namespace ToDoBot.Infrastructure.Persistence.Repository;

public record Repository(IDbContextFactory<DatabaseContext> DbContextFactory) : IRepository
{
    public ITransaction<TEntity> BeginTransaction<TEntity>()
        where TEntity : EntityBase =>
        new Transaction<TEntity>(DbContextFactory.CreateDbContext());

    public async Task<ITransaction<TEntity>> BeginTransactionAsync<TEntity>(CancellationToken cancellationToken)
        where TEntity : EntityBase =>
        new Transaction<TEntity>(await DbContextFactory.CreateDbContextAsync(cancellationToken));
}