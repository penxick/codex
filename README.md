# Testcord

Testcord is a desktop-first communication platform built with WPF on .NET 8, ASP.NET Core, MySQL, SignalR, and a clean modular architecture intended for long-term product growth.

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

Stage 1 establishes the solution, project structure, desktop shell, server startup, MySQL container, EF Core persistence wiring, and the first migration baseline.

Current audit status:

- Confirmed locally: solution builds, backend starts, `GET /health` returns `200`, `GET /swagger` returns `200` in Development, WPF client builds and launches.
- Not confirmed locally: migration application to a live database, because local MySQL server is not yet available on this machine.

## Projects

- `Testcord.Desktop`: WPF desktop client with MVVM, DI, configuration, and a Discord-like shell layout.
- `Testcord.Server`: ASP.NET Core Web API with SignalR, EF Core wiring, modular folders, and health endpoints.
- `Testcord.Shared`: Shared DTOs and contracts exchanged between client and server.

## Prerequisites

- .NET SDK 8.0+
- Local MySQL Server 8.x

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

## Configure Local MySQL

```powershell
server=localhost;port=3306;database=testcord;user=root;password=1234
```

1. Install MySQL locally.
2. Create a root password or another account with permission to create databases.
3. You do not have to create `testcord` manually if the configured user can create databases. The backend startup and `dotnet dotnet-ef database update` will create it through EF migrations.
4. Update `server/Testcord.Server/appsettings.json` or set `TESTCORD_ConnectionStrings__DefaultConnection`.
5. Ensure MySQL is listening on `localhost:3306`.

## Restore Dependencies

```powershell
dotnet restore
```

## Apply EF Core Migration

```powershell
dotnet dotnet-ef database update --project .\server\Testcord.Server\Testcord.Server.csproj --startup-project .\server\Testcord.Server\Testcord.Server.csproj --no-build
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

## Stage 1 Blocker

The remaining blocker is infrastructure verification:

- A local MySQL server is not currently installed or reachable on `localhost:3306`.
- Because MySQL is not running, `dotnet dotnet-ef database update` cannot complete against a live database.
- The backend now attempts to apply EF Core migrations on startup, so once local MySQL is reachable the database can be created automatically.
- Until that is fixed, Stage 1 is only partially complete.

## Next Step

After local MySQL is installed and reachable, rerun the migration, verify `GET /api/health` shows database connectivity, and only then move to Auth.
