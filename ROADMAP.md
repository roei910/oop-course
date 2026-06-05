# FinanceGrid Roadmap

> Living document. See AGENTS.md → "Keeping ROADMAP.md Updated" for rules.

## Status legend
✅ Done · 🚧 In Progress · 📋 Planned · 🐛 Known Bug

## ✅ Recently Completed (last 3 months)

### 2026-Q2
- ✅ Program.cs refactor for WebApplicationFactory — commit `414ac48` (2026-06-05)
- ✅ 49 in-process integration tests across 3 services — commit `b8ccc42` (2026-06-05)
  - FinancialData.IntegrationTests: 9 tests
  - Users.IntegrationTests: 26 tests
  - Webhook.IntegrationTests: 7 tests
- ✅ 15 system tests with Docker Compose — commit `b8ccc42` (2026-06-05)
  - Health checks, user/stock/webhook flows, cross-service scenarios
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

### 2026-Q3 — Production bugs
- 📋 Fix `NotSupportedException` × 4 in `StockRepository.cs:39-56` (UpdateStocksBySymbol, UpdateStocksAnalysis, AddNotification, RemoveNotification)
- 📋 Fix `NotSupportedException` × 6 in `UserRepository.cs:79-107` (notifications + stock notes)
- 📋 Fix date-dependent unit test `StockMarketTimeServiceTests.LastMarketCloseDateTime_ReturnsPastDate`
- 📋 Fix Angular test HTTP leak — remove hardcoded `oop-course.onrender.com` in 2 env files
- 📋 Clean up orphaned `.git/worktrees/glowing-knight` and `sunny-circuit`

### 2026-Q3 — CI/CD
- 📋 Add GitHub Actions workflow `ci-backend.yml` — `dotnet test --filter Category!=System`
- 📋 Add `ci-frontend.yml` — `npm test` (headless Chrome)
- 📋 Add `ci-system-tests.yml` (self-hosted runner, Docker required, `MANAGE_DOCKER_COMPOSE=true`)
- 📋 Configure coverlet thresholds: 70% Application, 50% Infrastructure

### 2026-Q4 — Test coverage gaps
- 📋 Add Gateway integration tests (`WebApplicationFactory<FinanceGrid.Gateway.Program>`)
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

| # | Bug | Location | Discovered | Tracking |
|---|-----|----------|------------|----------|
| 1 | `NotSupportedException` `UpdateStocksBySymbolAsync` | `StockRepository.cs:39` | 2026-Q2 | 2026-Q3 |
| 2 | `NotSupportedException` `UpdateStocksAnalysisAsync` | `StockRepository.cs:44` | 2026-Q2 | 2026-Q3 |
| 3 | `NotSupportedException` `AddNotificationAsync` (Stock) | `StockRepository.cs:50` | 2026-Q2 | 2026-Q3 |
| 4 | `NotSupportedException` `RemoveNotificationAsync` (Stock) | `StockRepository.cs:54` | 2026-Q2 | 2026-Q3 |
| 5 | `NotSupportedException` `AddNotificationAsync` (User) | `UserRepository.cs:79` | 2026-Q2 | 2026-Q3 |
| 6 | `NotSupportedException` `RemoveNotificationAsync` (User) | `UserRepository.cs:83` | 2026-Q2 | 2026-Q3 |
| 7 | `NotSupportedException` `ShowNotificationAsync` (User) | `UserRepository.cs:88` | 2026-Q2 | 2026-Q3 |
| 8 | `NotSupportedException` `AddStockNoteAsync` | `UserRepository.cs:93` | 2026-Q2 | 2026-Q3 |
| 9 | `NotSupportedException` `RemoveStockNoteAsync` | `UserRepository.cs:98` | 2026-Q2 | 2026-Q3 |
| 10 | `NotSupportedException` `UpdateStockNoteAsync` | `UserRepository.cs:103` | 2026-Q2 | 2026-Q3 |
| 11 | Date-dependent unit test failure | `StockMarketTimeServiceTests.cs:100` | pre-2026-Q2 | 2026-Q3 |
| 12 | Angular test HTTP leak to production | `environment.production.ts:3`, `environment.development.ts:3` | pre-2026-Q2 | 2026-Q3 |
| 13 | Orphaned worktrees (permission errors) | `.git/worktrees/glowing-knight`, `sunny-circuit` | 2026-Q2 | 2026-Q3 |

## 🧭 Architectural Decisions

- **In-memory SQLite** (not EF in-memory) — more realistic, supports constraints
- **Shared `SqliteConnection`** opened once per factory
- **Local DTOs in SystemTests** — services as black boxes
- **`IClassFixture<T>`** — one factory per test class
- **Real `PASSWORD_SALT`** in test seeding — matches production hashing
