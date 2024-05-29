using Krevetki.ToDoBot.Bot;

using Microsoft.EntityFrameworkCore;

using ToDoBot.Application;
using ToDoBot.Infrastructure;
using ToDoBot.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
       .AddJsonFile("appsettings.json", true, true)
       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
       .AddEnvironmentVariables()
       .AddUserSecrets<Program>(true);

builder.Services
       .AddInfrastructure(builder.Configuration)
       .AddApplication()
       .ConfigureServices();

var app = builder.Build();

var factory = app.Services.GetRequiredService<IDbContextFactory<DatabaseContext>>();
await using var context = factory.CreateDbContext();
await context.Database.MigrateAsync(app.Lifetime.ApplicationStopping);

app.Run();
