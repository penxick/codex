# Testcord

Testcord is a desktop-first communication platform built with WPF on .NET 8, ASP.NET Core, MySQL, SignalR, and a clean modular architecture intended for long-term product growth.

## Repository Layout

```text
testcord/
├── client/
│   └── Testcord.Desktop/
├── server/
│   └── Testcord.Server/
├── shared/
│   └── Testcord.Shared/
├── infrastructure/
├── docker/
│   └── mysql/
├── docs/
├── AGENTS.md
├── README.md
└── docker-compose.yml
```

## Stage 1 Scope

Stage 1 establishes the solution, project structure, desktop shell, server startup, MySQL container, EF Core persistence wiring, and the first migration baseline.

Current audit status:

- Confirmed locally: solution builds, backend starts, `GET /health` returns `200`, `GET /swagger` returns `200` in Development, WPF client builds and launches.
- Not confirmed locally: MySQL container startup and migration application to a live database, because the MySQL Docker image pull currently fails with `EOF` during layer download.

## Projects

- `Testcord.Desktop`: WPF desktop client with MVVM, DI, configuration, and a Discord-like shell layout.
- `Testcord.Server`: ASP.NET Core Web API with SignalR, EF Core wiring, modular folders, and health endpoints.
- `Testcord.Shared`: Shared DTOs and contracts exchanged between client and server.

## Prerequisites

- .NET SDK 8.0+
- Docker Desktop
- MySQL container support through Docker Compose

## Configuration

Server settings live in:

- `server/Testcord.Server/appsettings.json`
- `server/Testcord.Server/appsettings.Development.json`
- environment variables prefixed with `TESTCORD_`

Client settings live in:

- `client/Testcord.Desktop/appsettings.json`

Important server environment variables:

- `TESTCORD_ConnectionStrings__MySql`
- `TESTCORD_Smtp__Host`
- `TESTCORD_Smtp__Port`
- `TESTCORD_Smtp__UserName`
- `TESTCORD_Smtp__Password`
- `TESTCORD_Smtp__FromEmail`
- `TESTCORD_Smtp__FromName`

## Run MySQL

```powershell
docker compose up -d mysql
```

The default database name is `testcord`.

Important: Stage 1 is not fully complete until Docker Desktop is running, the MySQL image pulls successfully, and the container actually starts.

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

- Docker daemon is reachable only with elevated access, but pulling `mysql:8.4` currently fails with `EOF` during image download.
- Because MySQL is not running, `dotnet dotnet-ef database update` cannot complete against a live database.
- Until that is fixed, Stage 1 is only partially complete.

## Next Step

After Docker/MySQL is confirmed locally, rerun the migration, verify `GET /api/health` shows database connectivity, and only then move to Auth.
