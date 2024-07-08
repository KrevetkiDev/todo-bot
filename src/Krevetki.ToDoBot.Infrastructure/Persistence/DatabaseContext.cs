using Krevetki.ToDoBot.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Krevetki.ToDoBot.Infrastructure.Persistence;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Notification> Notifications { get; set; }

    public DbSet<ToDoItem> ToDoItems { get; set; }

    public DbSet<CallbackData> CallbackData { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> option)
        : base(option)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDoItem>().Property(x => x.Status).HasConversion<string>();
        modelBuilder.Entity<User>().Property(x => x.EveningNotificationStatus).HasConversion<string>();
    }
}
