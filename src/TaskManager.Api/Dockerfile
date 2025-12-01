FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj y restaurar paquetes
COPY ["src/TaskManager.Api/TaskManager.Api.csproj", "src/TaskManager.Api/"]
RUN dotnet restore "src/TaskManager.Api/TaskManager.Api.csproj"

# Copiar todo el código y publicar
COPY . .
WORKDIR /src/src/TaskManager.Api
RUN dotnet publish "TaskManager.Api.csproj" -c Release -o /app/publish /p:PublishTrimmed=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.Api.dll"]