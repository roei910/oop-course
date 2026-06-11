# FinanceGrid

Full-stack stock monitor: **Angular 16** (PrimeNG/Chart.js) frontend, **.NET 8** Clean Architecture microservices, **Aspire 13** orchestration, **RabbitMQ** event bus, **SQLite** (dev) / **PostgreSQL** (prod).

## Repo map

```
FinanceGrid/
├── FinanceGrid.slnx               # 21 projects
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
docker-compose.yml          # Profiles: sqlite (7 containers) | postgres (8 containers)
```

## Roadmap

See [ROADMAP.md](./ROADMAP.md) for project status, planned work, and known bugs.

### Keeping ROADMAP.md Updated

This is a **living document**. Update it whenever you:

1. **Complete an item** — Move from `📋 Planned` or `🚧 In Progress` to `✅ Recently Completed` with date (YYYY-QX) and commit SHA (first 7 chars)
2. **Start work** — Move from `📋 Planned` to `🚧 In Progress`
3. **Discover a bug** — Add to `🐛 Known Bugs` table with file:line, date, and tracking quarter
4. **Plan new work** — Add to `📋 Planned` with target quarter

**Rules:** Update in the same commit that completes the work. History window: keep last 3 months in "Recently Completed"; older items move to a one-line "Earlier milestones" entry. Maximum 150 lines.

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
docker-compose up                                   # SQLite (default)
DATABASE_PROVIDER=PostgreSQL docker-compose --profile postgres up   # PostgreSQL
```

| Service | Container | Port | Profile |
|---------|-----------|------|---------|
| Frontend | `financegrid` | 4200 | default |
| Gateway | `gateway` | 5000 | default |
| FinancialData Service | `financialdata-service` | 5001 | default |
| Users Service | `users-service` | 5002 | default |
| Webhook Service | `webhook-service` | 5003 | default |
| Webhook Processing | `webhook-processing` | — | default |
| RabbitMQ | `rabbitmq` | 5672, 15672 | default |
| PostgreSQL | `postgres` | 5432 | postgres |

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
- **Dual database**: `DatabaseProvider` config key selects SQLite (dev) or PostgreSQL (prod). Configured via `.env`, UserSecrets, or env vars. Two Docker Compose profiles: `sqlite` and `postgres`.
- **Auth**: Custom password hasher (`PASSWORD_SALT`), no JWT/OAuth.
- **Frontend**: Hash-based routing (`#/stocks/search`). `connectedUserGuard` protects `/user/*` and `/insights`. Interceptors: `interceptConnection`, `interceptLoader`.

## Database Provider

Set `DatabaseProvider` to `"SQLite"` or `"PostgreSQL"` via:
- **Docker**: `DatabaseProvider` in `.env` file
- **Local .NET**: `dotnet user-secrets set "DatabaseProvider" "PostgreSQL"`
- **Environment variable**: `DatabaseProvider=PostgreSQL`

Connection strings go in `ConnectionStrings:<ServiceName>` (same precedence rules).

Secrets are loaded in order: env vars → UserSecrets → .env → defaults. `.env` is gitignored.

## Testing

- Frontend: `ng test` (Karma + Jasmine, headless Chrome)
- Backend in-process: `dotnet test --filter Category!=System` (49 tests)
- System: `dotnet test --filter Category=System` (15 tests, requires Docker, set `MANAGE_DOCKER_COMPOSE=true`)
- See ROADMAP.md → "Test coverage gaps" for what's NOT yet covered
