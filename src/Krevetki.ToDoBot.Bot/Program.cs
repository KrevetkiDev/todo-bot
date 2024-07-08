using Krevetki.ToDoBot.Application;
using Krevetki.ToDoBot.Bot;
using Krevetki.ToDoBot.Infrastructure;
using Krevetki.ToDoBot.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

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
