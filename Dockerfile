# Базовый образ с ASP.NET Core для запуска
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Образ с SDK для сборки проекта
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "CoffeeMap/CoffeeMap.csproj"
RUN dotnet publish "CoffeeMap/CoffeeMap.csproj" -c Release -o /app/publish


# Финальный образ для запуска
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CoffeeMap.dll"]
