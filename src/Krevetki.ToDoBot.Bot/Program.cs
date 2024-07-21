using Krevetki.ToDoBot.Application;
using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.ToDoItems.Queries.GetTodoitemsByDate;
using Krevetki.ToDoBot.Bot;
using Krevetki.ToDoBot.Infrastructure;
using Krevetki.ToDoBot.Infrastructure.Persistence;

using MediatR;

using Microsoft.AspNetCore.Http.HttpResults;
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

app.MapPost(
       "/api/v1/todoitems",
       async (GetTodoItemsByDateQuery request,ISender sender,  CancellationToken cancellationToken) =>
       {
           var responseData = await sender.Send(request, cancellationToken);

           if (responseData.Count == 0)
               return Results.NoContent();

           return Results.Ok(responseData);
       })
   .WithName("GetToDoItemsByDate")
   .WithDescription("Get ToDo items by date")
   .WithOpenApi();

app.MapGet("$/api/v1/user/{user.id}/todoitems", async (GetTodoItemsByDateQuery request,ISender sender,  CancellationToken cancellationToken) =>
                                                {
                                                    var responseData = await sender.Send(request, cancellationToken);

                                                    if (responseData.Count == 0)
                                                        return Results.NoContent();

                                                    return Results.Ok(responseData);
                                                })
   .WithName("GetToDoItemsByUserId")
   .WithDescription("Get ToDo items by user id")
   .WithOpenApi();

app.Run();
