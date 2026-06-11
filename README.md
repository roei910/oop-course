# FinanceGrid

Full-stack stock monitor with .NET 8 microservices and Angular frontend.

# running the project
Run the backend services in C# .NET, run the frontend project in Angular (Node version 20 or higher).

# deployed version
- frontend: https://stock-collection.netlify.app/#/
- frontend option2: https://oop-course-frontend.onrender.com/#/
- backend swagger: https://oop-course.onrender.com/swagger

# running backend

```powershell
# Aspire (runs all services + dashboard)
dotnet run --project FinanceGrid\FinanceGrid.AppHost

# Or run each service individually:
dotnet run --project FinanceGrid\FinanceGrid.Gateway
dotnet run --project FinanceGrid\FinancialData\FinanceGrid.FinancialData.Api
dotnet run --project FinanceGrid\Users\FinanceGrid.Users.Api
dotnet run --project FinanceGrid\Webhook\FinanceGrid.Webhook.Api
dotnet run --project FinanceGrid\FinanceGrid.Webhook.Processing
```

# running frontend
```powershell
cd FinanceGrid.WebClient
npm install
npm start
```

There is an option to use configurations like `local`, `development`, and `production` which change the backend server URL.

# versions
- npm @9.8.1 or higher
- node @18.16.0 or higher
- angular @16.1.0 or higher
- .NET 8

---

# Database Configuration

The FinanceGrid microservices support both **SQLite** (default) and **PostgreSQL** databases.

## Quick Start

### SQLite (default — no setup needed)
```bash
docker-compose up
```

### PostgreSQL
```bash
DATABASE_PROVIDER=PostgreSQL docker-compose --profile postgres up
```

## Configuration via .env File

Copy `.env.example` to `.env` and edit:

```bash
cp .env.example .env
```

The `.env` file is **gitignored** — never commit it. Edit `.env.example` for shared changes.

| Variable | Description | Default |
|----------|-------------|---------|
| `DatabaseProvider` | `SQLite` or `PostgreSQL` | `SQLite` |
| `FINANCIALDATA_CONNECTION_STRING` | FinancialData connection | `Data Source=/data/FinancialData.db` |
| `USERS_CONNECTION_STRING` | Users connection | `Data Source=/data/Users.db` |
| `WEBHOOK_CONNECTION_STRING` | Webhook connection | `Data Source=/data/Webhook.db` |
| `POSTGRES_USER` | PostgreSQL username | `financegrid` |
| `POSTGRES_PASSWORD` | PostgreSQL password | (set in `.env`) |
| `POSTGRES_DB` | PostgreSQL database | `financegrid` |

## Secrets Management

The apps load configuration in this priority order:
1. **Environment variables** (highest — used in Docker/production)
2. **User Secrets** (local .NET development via `dotnet user-secrets`)
3. **.env file** (Docker Compose only)
4. **appsettings.json** (lowest)

### Local Development with User Secrets
```powershell
# Initialize secrets for a service
dotnet user-secrets init --project FinanceGrid\Users\FinanceGrid.Users.Api

# Set a connection string
dotnet user-secrets set "ConnectionStrings:Users" "Host=localhost;Port=5432;Database=Users;Username=me;Password=secret" --project FinanceGrid\Users\FinanceGrid.Users.Api
```

### Production
For production, use a secrets manager:
- **Azure**: Azure Key Vault with `AddAzureKeyVault()`
- **AWS**: AWS Secrets Manager
- **Docker**: Docker secrets or env vars injected by orchestrator

## Docker Compose Profiles

The default `docker-compose up` starts everything in **SQLite** mode. To switch to **PostgreSQL**, set `DATABASE_PROVIDER=PostgreSQL` and start the postgres container via the `postgres` profile:

| Mode | Database | Usage |
|------|----------|-------|
| Default | SQLite (files in volumes) | `docker-compose up` |
| Postgres | PostgreSQL container | `DATABASE_PROVIDER=PostgreSQL docker-compose --profile postgres up` |

The same service images run in both modes — only the connection string and the postgres container differ.

---

# EF Core Migrations

Each service has a `Migrations/` folder with an `InitialPostgreSQL` migration. SQLite uses `EnsureCreated()` for now (no migrations needed — file-based).

## Generate New Migrations

```bash
# For a new SQLite migration
dotnet ef migrations add MyChange \
  -p FinanceGrid/FinancialData/FinanceGrid.FinancialData.Infrastructure \
  -s FinanceGrid/FinancialData/FinanceGrid.FinancialData.Api \
  --context FinancialDataDbContext \
  -o Migrations

# For a new PostgreSQL migration (use a different folder to avoid clashes)
dotnet ef migrations add MyChangePostgreSQL \
  -p FinanceGrid/FinancialData/FinanceGrid.FinancialData.Infrastructure \
  -s FinanceGrid/FinancialData/FinanceGrid.FinancialData.Api \
  --context FinancialDataDbContext \
  -o Migrations
```

## Apply Migrations

```bash
# SQLite — uses EnsureCreated(), no manual migration needed
# PostgreSQL — apply at startup or via:
dotnet ef database update \
  -p FinanceGrid/FinancialData/FinanceGrid.FinancialData.Infrastructure \
  -s FinanceGrid/FinancialData/FinanceGrid.FinancialData.Api
```

## Important Notes

- **StocksApi** uses MongoDB and is not affected by this configuration
- **Data Migration**: No automatic migration between SQLite and PostgreSQL; use a custom ETL script
- **Testing**: Integration tests should cover both SQLite and PostgreSQL providers
- **Never commit secrets**: `.env` and `secrets.json` are gitignored