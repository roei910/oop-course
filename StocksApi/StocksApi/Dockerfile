FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["StocksApi/StocksApi.csproj", "StocksApi/"]
COPY ["SharedLibrary/SharedLibrary.csproj", "SharedLibrary/"]
COPY ["StocksAbstractions/StocksAbstractions.csproj", "StocksAbstractions/"]
COPY ["UsersAbstractions/UsersAbstractions.csproj", "UsersAbstractions/"]
COPY ["StocksLibrary/StocksLibrary.csproj", "StocksLibrary/"]
COPY ["UsersLibrary/UsersLibrary.csproj", "UsersLibrary/"]
COPY ["StocksProvider/StocksProvider.csproj", "StocksProvider/"]
COPY ["StocksApi.Providers.Mongo/StocksApi.Providers.Mongo.csproj", "StocksApi.Providers.Mongo/"]
COPY ["StocksApi.Providers.Sqlite/StocksApi.Providers.Sqlite.csproj", "StocksApi.Providers.Sqlite/"]
COPY ["StocksApi.Providers.UsersSqlite/StocksApi.Providers.UsersSqlite.csproj", "StocksApi.Providers.UsersSqlite/"]

RUN dotnet restore "StocksApi/StocksApi.csproj"
COPY . .
WORKDIR "/src/StocksApi"
RUN dotnet build "StocksApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StocksApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StocksApi.dll"]
