# Stocks Collection App

Full-stack stock monitor: **Angular 16** (PrimeNG/Chart.js) frontend, **.NET 8** microservices + legacy monolith, **MongoDB** + **SQLite**.

## Repo map

```
StocksApi/
├── StocksApi.sln          # 13 projects (including docker-compose .dcproj)
├── Gateway/               # YARP reverse proxy — port 5000, entry point for all APIs
├── StocksService/         # Stock microservice — port 5001, SQLite (stocks.db)
├── UsersService/          # User microservice — port 5002, SQLite (Users.db)
├── StocksApi/             # Legacy monolith — ports 7173/5108, Mongo or SQLite
├── StocksProvider/        # Yahoo Finance API integration
├── SharedLibrary/         # Config keys, shared DTOs
├── StocksAbstractions/    # Stock-domain interfaces
├── UsersAbstractions/     # User-domain interfaces
├── StocksLibrary/         # Stock-domain implementations
├── UsersLibrary/          # User-domain implementations
├── StocksApi.Providers.Mongo/       # Stock+User DALs (Mongo)
├── StocksApi.Providers.Sqlite/      # Stock DALs (SQLite)
├── StocksApi.Providers.UsersSqlite/ # User DALs (SQLite)
├── Library/               # ⚠️ Detached from .sln (leftover dir)
└── StocksApi.Abstractions/ # ⚠️ Detached from .sln (leftover dir)
client-angular/            # Angular 16 frontend, hash-based routing
docker-compose.yml         # Full stack: frontend:4200, gateway:5000, mongo:27017
```

## Required versions

- Node 18+, npm 9+
- .NET 8
- Angular CLI 16

## Commands

### Frontend (`client-angular/`)

```powershell
npm install
npm start                   # ng serve — default config: "development" (uses production backend URL)
ng serve -c local           # Use local backend at https://localhost:5000/api
npm run build -- --configuration=production  # or development | local
ng test                     # Karma + Jasmine (requires Chrome)
```

Build configs swap `src/environments/environment.ts`:
- `local` → `https://localhost:5000/api` (Gateway)
- `development` / `production` → `https://oop-course.onrender.com/api`

### Backend (`StocksApi/`)

Each service needs its own terminal:

```powershell
dotnet run --project StocksApi\Gateway          # Port 5000 — API entry point
dotnet run --project StocksApi\StocksService    # Port 5001 — requires dotnet user-secrets
dotnet run --project StocksApi\UsersService     # Port 5002 — requires dotnet user-secrets
dotnet run --project StocksApi\StocksApi        # Port 7173/5108 — legacy monolith
```

- Services redirect `/` → `/swagger`
- `dotnet user-secrets set CONNECTION_STRING "<mongodb-connection-string>"` for monolith
- Config: `appsettings.json` + `appsettings.{Environment}.json` + env vars

### Docker

```powershell
docker-compose up   # 6 containers from root docker-compose.yml
```

| Service | Container | Port |
|---------|-----------|------|
| Frontend | `stocks-collection` | 4200 |
| Gateway | `gateway` | 5000 |
| Stocks Service | `stocks-service` | 5001 |
| Users Service | `users-service` | 5002 |
| Monolith | `stocksapi` | 5108 |
| MongoDB | `mongodb` | 27017 |

Secrets: `./StocksApi/StocksApi/secrets.json` → `/etc/secrets/secrets.json`

## Architecture

- **Gateway (YARP)**: Single REST entry point. Routes `/api/stock/*` → StocksService, `/api/user/*`, `/api/share/*`, `/api/userStockWatch/*` → UsersService.
- **StocksService**: Stock-domain only. Yahoo Finance background updaters (`StocksAutomaticUpdater`, `StockAnalysisUpdater`).
- **UsersService**: User-domain only. `IStockRepository` is stubbed (throws `NotSupportedException`).
- **Legacy monolith**: Still functional, handles cross-domain stock-notification endpoints (will be replaced by RabbitMQ).
- **Auth**: Custom password hasher (`PASSWORD_SALT`), no JWT/OAuth.
- **Frontend**: Hash-based routing (`#/stocks/search`). `connectedUserGuard` protects `/user/*` and `/insights`. Interceptors: `interceptConnection`, `interceptLoader`.

## Secrets

- Dev: `dotnet user-secrets` with key `CONNECTION_STRING`
- Docker/prod: env var `SECRETS_FILE_PATH` pointing to JSON with `ConnectionString` key
- `secrets.json` gitignored; see `secrets-example.json`
- `appsettings.json` has hardcoded RapidAPI keys (committed — learning project)

## Testing

- Frontend only: `ng test` (Karma + Jasmine, minimal spec files)
- No backend tests
