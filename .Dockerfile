FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/ApiEstagioBicicletaria/ApiEstagioBicicletaria.csproj", "src/ApiEstagioBicicletaria/"]
RUN dotnet restore "src/ApiEstagioBicicletaria/ApiEstagioBicicletaria.csproj"
COPY src/ApiEstagioBicicletaria/ src/ApiEstagioBicicletaria/
RUN dotnet publish "src/ApiEstagioBicicletaria/ApiEstagioBicicletaria.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiEstagioBicicletaria.dll"]