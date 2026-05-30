FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["StocksService/StocksService.csproj", "StocksService/"]
COPY ["SharedLibrary/SharedLibrary.csproj", "SharedLibrary/"]
COPY ["StocksAbstractions/StocksAbstractions.csproj", "StocksAbstractions/"]
COPY ["UsersAbstractions/UsersAbstractions.csproj", "UsersAbstractions/"]
COPY ["StocksLibrary/StocksLibrary.csproj", "StocksLibrary/"]
COPY ["StocksProvider/StocksProvider.csproj", "StocksProvider/"]
COPY ["StocksApi.Providers.Sqlite/StocksApi.Providers.Sqlite.csproj", "StocksApi.Providers.Sqlite/"]

RUN dotnet restore "StocksService/StocksService.csproj"
COPY . .
WORKDIR "/src/StocksService"
RUN dotnet build "StocksService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StocksService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StocksService.dll"]
