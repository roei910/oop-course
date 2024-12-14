FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["StocksApi/StocksApi.csproj", "StocksApi/"]
COPY ["YahooFinance/YahooFinance.csproj", "YahooFinance/"]
COPY ["Library/Library.csproj", "Library/"]

RUN dotnet dev-certs https --trust
RUN dotnet restore "StocksApi/StocksApi.csproj"
COPY . .
WORKDIR "/src/StocksApi"
RUN dotnet build "StocksApi.csproj" -c Release -o /app/buildx

FROM build AS publish
RUN dotnet publish "StocksApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StocksApi.dll"]