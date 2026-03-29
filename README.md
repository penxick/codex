# Testcord

Testcord is a desktop-first communication platform built with WPF on .NET 8, ASP.NET Core, PostgreSQL, SignalR, and a clean modular architecture intended for long-term product growth.

## Repository Layout

```text
testcord/
|-- client/
|   `-- Testcord.Desktop/
|-- server/
|   `-- Testcord.Server/
|-- shared/
|   `-- Testcord.Shared/
|-- infrastructure/
|-- docs/
|-- AGENTS.md
|-- README.md
`-- Testcord.sln
```

## Stage 1 Scope

Stage 1 establishes the solution, project structure, desktop shell, server startup, PostgreSQL connectivity, EF Core persistence wiring, and the first migration baseline.

Current status:

- Confirmed locally: solution builds, PostgreSQL migration is applied, backend starts, `GET /health` returns `200`, `GET /swagger` returns `200` in Development, `GET /api/health` returns `200`, and the WPF client builds and launches.

## Projects

- `Testcord.Desktop`: WPF desktop client with MVVM, DI, configuration, and a Discord-like shell layout.
- `Testcord.Server`: ASP.NET Core Web API with SignalR, EF Core wiring, modular folders, and health endpoints.
- `Testcord.Shared`: Shared DTOs and contracts exchanged between client and server.

## Prerequisites

- .NET SDK 8.0+
- Local PostgreSQL Server 18.x

## Configuration

Server settings live in:

- `server/Testcord.Server/appsettings.json`
- `server/Testcord.Server/appsettings.Development.json`
- environment variables prefixed with `TESTCORD_`

Client settings live in:

- `client/Testcord.Desktop/appsettings.json`

Important server environment variables:

- `TESTCORD_ConnectionStrings__DefaultConnection`
- `TESTCORD_Smtp__Host`
- `TESTCORD_Smtp__Port`
- `TESTCORD_Smtp__UserName`
- `TESTCORD_Smtp__Password`
- `TESTCORD_Smtp__FromEmail`
- `TESTCORD_Smtp__FromName`

## Configure Local PostgreSQL

```powershell
Host=localhost;Port=5432;Database=testcord;Username=postgres;Password=PUT_PASSWORD_HERE
```

1. Ensure PostgreSQL is running on `localhost:5432`.
2. Create or use a `postgres` password.
3. Insert the password into `server/Testcord.Server/appsettings.json` or set `TESTCORD_ConnectionStrings__DefaultConnection`.
4. You do not have to create `testcord` manually if the configured user can create databases. The backend startup and `dotnet dotnet-ef database update` will create it through EF migrations.
5. If you prefer not to edit `appsettings.json`, set:

```powershell
$env:TESTCORD_ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=testcord;Username=postgres;Password=PUT_PASSWORD_HERE"
```

## Restore Dependencies

```powershell
dotnet restore
```

## Apply EF Core Migration

```powershell
dotnet dotnet-ef database update --project .\server\Testcord.Server\Testcord.Server.csproj --startup-project .\server\Testcord.Server\Testcord.Server.csproj --no-build
```

To verify created tables:

```powershell
& 'D:\postgresql\bin\psql.exe' -w -h localhost -p 5432 -U postgres -d testcord -tAc "SELECT tablename FROM pg_tables WHERE schemaname='public' ORDER BY tablename;"
```

## Run Server

```powershell
dotnet run --project .\server\Testcord.Server\Testcord.Server.csproj --launch-profile http
```

Useful endpoints:

- `GET /health`
- `GET /swagger` in Development
- `GET /api/health`
- `GET /api/bootstrap`

Swagger is intentionally enabled only in Development through `UseSwagger()` and `UseSwaggerUI()` so it is available for local debugging without being forced on in every environment.

## Run Desktop Client

```powershell
dotnet run --project .\client\Testcord.Desktop\Testcord.Desktop.csproj
```

## Release Build

Desktop executable:

```powershell
dotnet publish .\client\Testcord.Desktop\Testcord.Desktop.csproj -c Release -r win-x64 --self-contained false
```

Server:

```powershell
dotnet publish .\server\Testcord.Server\Testcord.Server.csproj -c Release
```

## Stage 1 Verification

The Stage 1 PostgreSQL foundation is verified locally:

- PostgreSQL is reachable on `localhost:5432`.
- Database `testcord` exists.
- EF Core migration `InitialPostgreSql` is applied.
- Backend `GET /api/health` reports a connected database.

## Next Step

Stage 2 can now begin with Auth.
