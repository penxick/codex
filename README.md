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

## Current Scope

Stage 1 established the solution, project structure, desktop shell, server startup, PostgreSQL connectivity, EF Core persistence wiring, and the first migration baseline.
Stage 2 has now started on the backend with the Auth module: registration, email verification, login, JWT access token issuance, and refresh token rotation.

Current status:

- Confirmed locally: solution builds, PostgreSQL migrations are applied, backend starts, `GET /health` returns `200`, `GET /swagger` returns `200` in Development, `GET /api/health` returns `200`, Auth endpoints complete an end-to-end register/verify/login/refresh flow, and the WPF client builds and launches.

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
- `TESTCORD_Jwt__Issuer`
- `TESTCORD_Jwt__Audience`
- `TESTCORD_Jwt__SigningKey`
- `TESTCORD_Jwt__AccessTokenMinutes`
- `TESTCORD_Jwt__RefreshTokenDays`
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
- `POST /api/auth/register`
- `POST /api/auth/verify-email`
- `POST /api/auth/login`
- `POST /api/auth/refresh`

Swagger is intentionally enabled only in Development through `UseSwagger()` and `UseSwaggerUI()` so it is available for local debugging without being forced on in every environment.

## Auth Module Verification

The backend Auth module is verified locally against PostgreSQL:

- Registration creates a user and an email verification record.
- Email verification succeeds with the generated code.
- Login returns JWT access and refresh tokens.
- Refresh rotates the refresh token and returns a fresh session payload.

Example request payloads:

```json
POST /api/auth/register
{
  "email": "user@testcord.local",
  "password": "Password123!",
  "nickname": "user123",
  "displayName": "User 123"
}
```

```json
POST /api/auth/verify-email
{
  "email": "user@testcord.local",
  "code": "123456"
}
```

```json
POST /api/auth/login
{
  "email": "user@testcord.local",
  "password": "Password123!"
}
```

```json
POST /api/auth/refresh
{
  "refreshToken": "PUT_REFRESH_TOKEN_HERE"
}
```

For local development, the SMTP sender currently logs the outgoing email body to the server log instead of talking to a real SMTP server. That makes it possible to read the verification code during local testing without adding a mail dependency yet.

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

Continue Stage 2 by wiring the authenticated desktop client flow, persistent session storage, and the first user profile endpoints on top of the verified Auth backend.
