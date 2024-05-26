
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace ToDoBot.Infrastructure.Persistence;

    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> option) : base(option)
        {
        }
    }
