using Krevetki.ToDoBot.Application;
using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Common.Models.Contracts;
using Krevetki.ToDoBot.Application.ToDoItems.Queries.ListTasksByDate;
using Krevetki.ToDoBot.Bot;
using Krevetki.ToDoBot.Domain.Entities;
using Krevetki.ToDoBot.Infrastructure;
using Krevetki.ToDoBot.Infrastructure.Persistence;

using MediatR;

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var factory = app.Services.GetRequiredService<IDbContextFactory<DatabaseContext>>();
await using var context = factory.CreateDbContext();
await context.Database.MigrateAsync(app.Lifetime.ApplicationStopping);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet(
       "/api/v1/todoitems",
       async (ISender sender, IRepository repository, TodoItemsByDateRequest request, CancellationToken cancellationToken) =>
       {
           await var trans
           User user = null; //get user

           var query = new ListTaskByDateQuery() { Date = request.DateTime, User = user };

           await sender.Send(query, cancellationToken);
       })
   .WithName("GetWeatherForecast")
   .WithOpenApi();

app.Run();
