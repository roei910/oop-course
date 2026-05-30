# FinanceGrid

Full-stack stock monitor: **Angular 16** (PrimeNG/Chart.js) frontend, **.NET 8** Clean Architecture microservices, **Aspire 13** orchestration, **RabbitMQ** event bus, **SQLite** (dev) / **PostgreSQL** (prod).

## Repo map

```
FinanceGrid/
├── FinanceGrid.slnx               # 17 projects
├── FinanceGrid.Shared/             # BaseEntity, ValueObject, DomainEvent, constants
├── FinanceGrid.Gateway/            # YARP reverse proxy — port 5000
├── FinanceGrid.ServiceDefaults/    # OpenTelemetry + health checks
├── FinanceGrid.AppHost/            # Aspire 13 orchestration (no workload needed)
├── FinanceGrid.Webhook.Processing/ # Background webhook delivery worker
├── FinancialData/                  # Clean Architecture: Domain → App → Infra → Api (port 5001)
│   ├── FinanceGrid.FinancialData.Domain/
│   ├── FinanceGrid.FinancialData.Application/
│   ├── FinanceGrid.FinancialData.Infrastructure/
│   └── FinanceGrid.FinancialData.Api/
├── Users/                          # Clean Architecture: Domain → App → Infra → Api (port 5002)
│   ├── FinanceGrid.Users.Domain/
│   ├── FinanceGrid.Users.Application/
│   ├── FinanceGrid.Users.Infrastructure/
│   └── FinanceGrid.Users.Api/
└── Webhook/                        # Clean Architecture: Domain → App → Infra → Api (port 5003)
    ├── FinanceGrid.Webhook.Domain/
    ├── FinanceGrid.Webhook.Application/
    ├── FinanceGrid.Webhook.Infrastructure/
    └── FinanceGrid.Webhook.Api/
StocksApi/                  # Legacy (preserved as snapshot)
FinanceGrid.WebClient/       # Angular 16 frontend, hash-based routing
docker-compose.yml          # 8 containers: frontend:4200, gateway:5000, rabbitmq:5672/15672
```

## Required versions

- Node 18+, npm 9+
- .NET 8
- Angular CLI 16

## Commands

### Frontend (`FinanceGrid.WebClient/`)

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

### Backend (`FinanceGrid/`)

Each service needs its own terminal:

```powershell
dotnet run --project FinanceGrid\FinanceGrid.AppHost              # Aspire orchestration (runs all)
dotnet run --project FinanceGrid\FinanceGrid.Gateway              # Port 5000
dotnet run --project FinanceGrid\FinancialData\FinanceGrid.FinancialData.Api  # Port 5001
dotnet run --project FinanceGrid\Users\FinanceGrid.Users.Api      # Port 5002
dotnet run --project FinanceGrid\Webhook\FinanceGrid.Webhook.Api  # Port 5003
dotnet run --project FinanceGrid\FinanceGrid.Webhook.Processing   # No HTTP (background worker)
```

### Aspire (runs all services + dashboard)

```powershell
dotnet run --project FinanceGrid\FinanceGrid.AppHost
```

### Docker

```powershell
docker-compose up   # 8 containers from root docker-compose.yml
```

| Service | Container | Port |
|---------|-----------|------|
| Frontend | `financegrid` | 4200 |
| Gateway | `gateway` | 5000 |
| FinancialData Service | `financialdata-service` | 5001 |
| Users Service | `users-service` | 5002 |
| Webhook Service | `webhook-service` | 5003 |
| Webhook Processing | `webhook-processing` | — |
| RabbitMQ | `rabbitmq` | 5672, 15672 |

### Build

```powershell
dotnet build FinanceGrid\FinanceGrid.slnx
```

## Architecture

- **Clean Architecture** per service: Domain (no deps) → Application (Domain only) → Infrastructure (implements Application) → Api (wires everything).
- **Gateway (YARP)**: Single REST entry point. Routes `/api/stock/*` → FinancialData, `/api/user/*`, `/api/share/*`, `/api/userStockWatch/*` → Users, `/api/webhook/*` → Webhook.
- **FinancialData Service**: Stock-domain only. Yahoo Finance background updaters, EF Core with SQLite/PostgreSQL.
- **Users Service**: User-domain only. Password hashing (SHA256 + salt), user stock watches.
- **Webhook Service**: External webhook subscriptions. Clients register URLs to receive push notifications.
- **Webhook Processing**: Background worker consuming RabbitMQ events, delivering webhooks with HMAC signatures and retry logic.
- **RabbitMQ**: Internal event bus for service-to-service communication (clients never connect directly).
- **Dual database**: `DatabaseProvider` config key selects SQLite (dev) or PostgreSQL (prod). Configured via `appsettings.json`.
- **Auth**: Custom password hasher (`PASSWORD_SALT`), no JWT/OAuth.
- **Frontend**: Hash-based routing (`#/stocks/search`). `connectedUserGuard` protects `/user/*` and `/insights`. Interceptors: `interceptConnection`, `interceptLoader`.

## Database Provider

Set `DatabaseProvider` to `"SQLite"` or `"PostgreSQL"` in `appsettings.json`. Connection string goes in `ConnectionStrings:<ServiceName>`.

## Testing

- Frontend only: `ng test` (Karma + Jasmine, minimal spec files)
- No backend tests
