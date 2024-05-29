using Microsoft.EntityFrameworkCore;

using ToDoBot.Domain;
using ToDoBot.Domain.Entities;

namespace ToDoBot.Infrastructure.Persistence;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> option)
        : base(option)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ToDoItem>().Property(x => x.Status).HasConversion<string>();
}
