FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
# Ensure the runtime knows the app is running inside a container so code
# that checks DOTNET_RUNNING_IN_CONTAINER can behave correctly.
ENV DOTNET_RUNNING_IN_CONTAINER=true

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia sólo los csproj y restaura para aprovechar cache
COPY ["src/TaskManager.Api/TaskManager.Api.csproj", "src/TaskManager.Api/"]
COPY ["src/TaskManager.Application/TaskManager.Application.csproj", "src/TaskManager.Application/"]
COPY ["src/TaskManager.Domain/TaskManager.Domain.csproj", "src/TaskManager.Domain/"]
COPY ["src/TaskManager.Infrastructure/TaskManager.Infrastructure.csproj", "src/TaskManager.Infrastructure/"]
RUN dotnet restore "src/TaskManager.Api/TaskManager.Api.csproj"

# Copiamos todo y compilamos/publish
COPY . .
WORKDIR "/src/src/TaskManager.Api"
RUN dotnet build "TaskManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManager.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Start the app directly. Docker Compose 'depends_on' and the DB healthcheck
# will help ensure the DB container is started; transient DB errors are handled
# by EF Core retry policies configured in the app.
ENTRYPOINT ["dotnet", "TaskManager.Api.dll"]
