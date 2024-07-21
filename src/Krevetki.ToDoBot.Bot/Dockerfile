FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Krevetki.ToDoBot.Bot/Krevetki.ToDoBot.Bot.csproj", "src/Krevetki.ToDoBot.Bot/"]
COPY ["src/Krevetki.ToDoBot.Infrastructure/Krevetki.ToDoBot.Infrastructure.csproj", "src/Krevetki.ToDoBot.Infrastructure/"]
COPY ["src/Krevetki.ToDoBot.Application/Krevetki.ToDoBot.Application.csproj", "src/Krevetki.ToDoBot.Application/"]
COPY ["src/Krevetki.ToDoBot.Domain/Krevetki.ToDoBot.Domain.csproj", "src/Krevetki.ToDoBot.Domain/"]
RUN dotnet restore "src/Krevetki.ToDoBot.Bot/Krevetki.ToDoBot.Bot.csproj"
COPY . .
WORKDIR "/src/src/Krevetki.ToDoBot.Bot"
RUN dotnet build "Krevetki.ToDoBot.Bot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Krevetki.ToDoBot.Bot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Krevetki.ToDoBot.Bot.dll"]
