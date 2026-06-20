# FinanceGrid Roadmap

> Living document. See AGENTS.md → "Keeping ROADMAP.md Updated" for rules.

## Status legend
✅ Done · 🚧 In Progress · 📋 Planned · 🐛 Known Bug

> Last updated: 2026-06-15

## ✅ Recently Completed (last 3 months)

### 2026-Q2
- ✅ Production bug fixes — all 13 known bugs resolved (commit `039f583`)
  - Moved stock update logic from `StockRepository` to `StockService` (Clean Architecture)
  - Removed `UpdateStocksBySymbolAsync`, `UpdateStocksAnalysisAsync`, `AddNotificationAsync`, `RemoveNotificationAsync` from `IStockRepository`
  - Implemented `UpdateStocksAsync` and `UpdateStocksAnalysisAsync` in `StockRepository` (pure DB operations)
  - `StockService.UpdateStocksBySymbolAsync` now orchestrates: calls `IFinanceStrategy` → updates DB via `IStockRepository`
  - `StockService.UpdateStocksAnalysisAsync` now orchestrates: calls `IFinanceStrategy` → updates DB via `IStockRepository`
  - Removed 6 dead methods from `IUserRepository` (notifications + stock notes — no callers in new architecture)
  - Fixed `StockMarketTimeService.LastMarketCloseDateTime()` — added `DateTimeKind.Utc`
  - Fixed Angular test HTTP leak — `environment.development.ts` now points to `localhost:5000`
  - Updated 33 unit tests + 9 integration tests to match new architecture
- ✅ Configurable database provider hardening — secrets, profiles, migrations (commit `efb72b7`)
  - Added UserSecrets support to all 5 service projects
  - Renamed `.env` → `.env.example` (gitignored), placeholder passwords
  - Restructured `docker-compose.yml` with `sqlite` and `postgres` profiles
  - Added `depends_on: postgres` with healthcheck for PostgreSQL services
  - Fixed fallback connection string path (`/data/` in Docker)
  - Added initial PostgreSQL migrations for FinancialData, Users, Webhook
  - Added explicit `FinanceGrid.Shared` reference to `Webhook.Processing`
- ✅ Centralized database configuration with IOptions pattern — commit `57e4152` (2026-Q2)
  - Replaced `GetDatabaseConfiguration()` with `AddDatabaseConfiguration()` using `IOptions<DatabaseConfiguration>`
  - Database config binds to a `Database` section (`Provider` + `ConnectionString`)
  - All services use `IOptions<>` + `switch` for provider selection
  - SQLite defaults to `Data Source=/data/{ServiceName}.db` when no connection string
  - PostgreSQL requires explicit connection string (throws clear error if missing)
  - Docker Compose updated to `Database__Provider` + `Database__ConnectionString`
  - Removed unused `ConfigurationKeys.DatabaseProvider` and `ConfigurationKeys.ConnectionString`
- ✅ Extracted `FinanceGrid.Persistence` project for clean separation — commit `f39f339` (2026-Q2)
  - Created `FinanceGrid.Persistence` with `DbContextExtensions` (single source of truth)
  - `AddDbContext<T>` centralizes config binding + provider switch
  - `ApplyMigrations<T>` / `ApplyMigrationsAsync<T>` centralizes migration logic
  - `FinanceGrid.Shared` stays clean with no EF Core dependencies
  - `FinanceGrid.Gateway` stays clean (no transitive EF Core deps)
  - Renamed DI methods: `Add*Persistence` → `Add*Database`
  - Renamed composition roots: `Add*Infrastructure` → `Add*Services`
  - Removed 3 copies of DbContext factory + switch blocks, 4 copies of ApplyMigrations
  - All 151 tests pass (33 + 21 + 10 + 45 unit, 9 + 26 + 7 integration)
- ✅ Program.cs refactor for WebApplicationFactory — commit `414ac48` (2026-06-05)
- ✅ 49 in-process integration tests across 3 services — commit `b8ccc42` (2026-06-05)
  - FinancialData.IntegrationTests: 9 tests
  - Users.IntegrationTests: 26 tests
  - Webhook.IntegrationTests: 7 tests
- ✅ 15 system tests with Docker Compose — commit `b8ccc42` (2026-06-05)
  - Health checks, user/stock/webhook flows, cross-service scenarios
- ✅ Add Gateway integration tests — 7 tests covering health, Swagger, CORS, YARP routing (commit `TBD`)
- ✅ Fix Angular test suite — all 142 tests passing (commit `702bd08`)
  - Migrated all spec files from `HttpClientTestingModule` to `provideHttpClient()` + `provideHttpClientTesting()`
  - Added `karma.conf.js` with `ChromeHeadlessNoSandbox` launcher
  - Added test `fileReplacements` + `defaultConfiguration: test` in `angular.json`
  - Fixed `DockerComposeFixture.ComposeFile` path resolution from `AppContext.BaseDirectory`
  - Simplified `ci-system-tests.yml` — fixture manages Docker lifecycle
- ✅ Shared SqliteConnection pattern for in-memory EF Core
- ✅ Local DTOs in SystemTests (black-box testing pattern)

### 2026-Q1
- ✅ Aspire 13 orchestration wired up — commit `5665976` (2026-Q1)
- ✅ Clean Architecture: Domain → App → Infra → Api across 3 services

### 2025-Q4
- ✅ YARP gateway routing configured
- ✅ RabbitMQ event bus integrated
- ✅ Dual SQLite/PostgreSQL database provider

## Earlier milestones
*Pre-2025-Q4 work* — see git log `68b07ff..HEAD` (StocksApi snapshot, Aspire setup, initial Clean Architecture refactor).

## 🚧 In Progress
*(none)*

## 📋 Planned

### 2026-Q3 — CI/CD
- ✅ Add GitHub Actions workflow `ci-backend.yml` — `dotnet test --filter Category!=System` (commit `476b4a4`)
- ✅ Add `ci-frontend.yml` — `ng test --browsers ChromeHeadless` (commit `476b4a4`)
- ✅ Add `ci-system-tests.yml` — Docker Compose on GitHub-hosted runner, fixture manages lifecycle (commit `702bd08`)
- ✅ Configure coverlet thresholds: 70% Application, 50% Infrastructure (commit `476b4a4`)

### 2026-Q4 — Test coverage gaps
- ✅ Add Gateway integration tests (`WebApplicationFactory<FinanceGrid.Gateway.Program>`) — completed 2026-06-15
- 📋 Add Webhook.Processing worker tests (Testcontainers for RabbitMQ)
- 📋 Add contract tests between services (Pact or JSON snapshots)

### 2027-Q1 — Production readiness (v1.0)
- 📋 Switch default `DatabaseProvider` to PostgreSQL
- 📋 Add OpenAPI spec generation per service (Swashbuckle enrichment)
- 📋 Add JWT/OAuth (replace custom password hasher)
- 📋 Add rate limiting on gateway
- 📋 Add structured logging review (Serilog)

### 2027-Q2+ — Features (TBD)
- 📋 Real-time price streaming (SignalR / WebSocket)
- 📋 Portfolio analytics dashboard
- 📋 Multi-currency support
- 📋 Mobile app (MAUI)

## 🐛 Known Bugs

| # | Bug | Location | Discovered | Status |
|---|-----|----------|------------|--------|
| 1 | `NotSupportedException` `UpdateStocksBySymbolAsync` | `StockRepository.cs:39` | 2026-Q2 | ✅ Fixed — moved to `StockService` |
| 2 | `NotSupportedException` `UpdateStocksAnalysisAsync` | `StockRepository.cs:44` | 2026-Q2 | ✅ Fixed — moved to `StockService` |
| 3 | `NotSupportedException` `AddNotificationAsync` (Stock) | `StockRepository.cs:50` | 2026-Q2 | ✅ Fixed — removed (no callers) |
| 4 | `NotSupportedException` `RemoveNotificationAsync` (Stock) | `StockRepository.cs:54` | 2026-Q2 | ✅ Fixed — removed (no callers) |
| 5 | `NotSupportedException` `AddNotificationAsync` (User) | `UserRepository.cs:79` | 2026-Q2 | ✅ Fixed — removed (no callers) |
| 6 | `NotSupportedException` `RemoveNotificationAsync` (User) | `UserRepository.cs:83` | 2026-Q2 | ✅ Fixed — removed (no callers) |
| 7 | `NotSupportedException` `ShowNotificationAsync` (User) | `UserRepository.cs:88` | 2026-Q2 | ✅ Fixed — removed (no callers) |
| 8 | `NotSupportedException` `AddStockNoteAsync` | `UserRepository.cs:93` | 2026-Q2 | ✅ Fixed — removed (no callers) |
| 9 | `NotSupportedException` `RemoveStockNoteAsync` | `UserRepository.cs:98` | 2026-Q2 | ✅ Fixed — removed (no callers) |
| 10 | `NotSupportedException` `UpdateStockNoteAsync` | `UserRepository.cs:103` | 2026-Q2 | ✅ Fixed — removed (no callers) |
| 11 | Date-dependent unit test failure | `StockMarketTimeServiceTests.cs:100` | pre-2026-Q2 | ✅ Fixed — added `DateTimeKind.Utc` |
| 12 | Angular test HTTP leak to production | `environment.development.ts:3` | pre-2026-Q2 | ✅ Fixed — changed to `localhost:5000` |
| 13 | Orphaned worktrees (permission errors) | `.git/worktrees/glowing-knight`, `sunny-circuit` | 2026-Q2 | ✅ Fixed — already cleaned up |

## 🧭 Architectural Decisions

- **In-memory SQLite** (not EF in-memory) — more realistic, supports constraints
- **Shared `SqliteConnection`** opened once per factory
- **Local DTOs in SystemTests** — services as black boxes
- **`IClassFixture<T>`** — one factory per test class
- **Real `PASSWORD_SALT`** in test seeding — matches production hashing
