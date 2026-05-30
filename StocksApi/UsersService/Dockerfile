FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UsersService/UsersService.csproj", "UsersService/"]
COPY ["SharedLibrary/SharedLibrary.csproj", "SharedLibrary/"]
COPY ["UsersAbstractions/UsersAbstractions.csproj", "UsersAbstractions/"]
COPY ["UsersLibrary/UsersLibrary.csproj", "UsersLibrary/"]
COPY ["StocksApi.Providers.UsersSqlite/StocksApi.Providers.UsersSqlite.csproj", "StocksApi.Providers.UsersSqlite/"]
COPY ["StocksAbstractions/StocksAbstractions.csproj", "StocksAbstractions/"]

RUN dotnet restore "UsersService/UsersService.csproj"
COPY . .
WORKDIR "/src/UsersService"
RUN dotnet build "UsersService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UsersService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsersService.dll"]
