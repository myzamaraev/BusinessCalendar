# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

### Run (Docker)
```bash
# From Src/
docker compose -f compose-mongo.yml up --wait        # MongoDB backend
docker compose -f compose-postgres.yml up --wait     # Postgres backend
docker compose -f compose-mongo-auth.yml up --wait   # MongoDB + Keycloak (users: admin/admin, bc-user/bc-user, bc-manager/bc-manager)
docker compose -f compose-mongo.yml build --no-cache # Full rebuild
```
Service runs at http://localhost:4080.

### Run (local backend)
```bash
cd Src
dotnet run --project BusinessCalendar.WebAPI/BusinessCalendar.WebAPI.csproj
# Requires a running database — use docker for the DB and run the API locally
```

### Frontend dev server
```bash
cd Src/dashboard
npm install
npm run dev   # http://localhost:5173, proxies /bff/v1 to https://localhost:7034
npm run lint
```

### Tests
```bash
cd Src
dotnet test BusinessCalendar.sln
dotnet test BusinessCalendar.sln --filter "FullyQualifiedName~CalendarManagement"  # single test
```

**Test conventions:** Use FluentAssertions for all assertions in backend tests (e.g. `result.Should().Be(expected)`). Do not use NUnit's `Assert.That`.

## Architecture

**Layer overview:**
- `BusinessCalendar.Domain` — business logic only; no I/O. Defines domain services (`ICalendarManagementService`, `ICalendarIdentifierService`, `IClientCalendarService`) and storage interfaces (`ICalendarStorageService`, `ICalendarIdentifierStorageService`).
- `BusinessCalendar.WebAPI` — ASP.NET Core 9 host. Two controller groups:
  - `Controllers/BffV1/` — authenticated CRUD endpoints consumed by the Vue dashboard
  - `Controllers/ApiV1/` — public read-only API (used by client integrators)
- `BusinessCalendar.Postgres` / `BusinessCalendar.MongoDb` — swappable storage implementations. Selected at runtime via `appsettings.json` → `Storage:DatabaseType` (`Postgres` or `MongoDb`).
- `BusinessCalendar.SqlMigrations` — FluentMigrator migrations for Postgres, run automatically on startup when `Storage:AllowAutoMigrations: true`.
- `BusinessCalendar.Client` (netstandard2.0) — NuGet client library wrapping the public API.

**Dependency injection:** `ServiceCollectionExtensions.cs` in WebAPI wires everything up. `AddStorage()` switches between Postgres/MongoDB registrations; `AddBusinessCalendarDomain()` registers domain services.

**Error handling:** Hellang.Middleware.ProblemDetails maps `ClientException` → 400, FluentValidation failures → 400, unhandled → 500 (RFC 7807 format).

**Frontend:** Vue 3 + Vuex + Vue Router built with Vite. State split into Vuex modules under `dashboard/src/store/modules/` (auth, calendar, localization, navIdentifiers). In production the compiled SPA is served as static files from WebAPI's `/wwwroot`.

**Auth:** OpenID Connect via `Auth:UseOpenIdConnectAuth` flag. When disabled, no authentication is enforced. Keycloak compose file provides a preconfigured realm.

**Storage patterns:**
- Postgres: Dapper + raw SQL via `NpgsqlDataSource`
- MongoDB: native driver with upsert operations

**Key config files in `Src/`:** `.env`, `appsettings.json`, `logger.json`, `mongodb_settings.json`, `postgres_settings.json`
