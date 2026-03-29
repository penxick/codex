# Testcord Working Agreement

## Product Direction

Testcord is a long-term desktop communication platform inspired by Discord. We optimize for modularity, predictable delivery, and production-ready architecture instead of short-lived demos.

## Technology Stack

1. Desktop client: C#, .NET 8, WPF, MVVM, DI, HttpClient, SignalR client.
2. Server: ASP.NET Core Web API, SignalR, EF Core, JWT authentication.
3. Database: local PostgreSQL on `localhost:5432` using EF Core + Npgsql.
4. Shared contracts: .NET class library for DTOs and transport models.
5. Electron/Node are not the primary runtime for this codebase and should not be reintroduced as the main app path.

## Delivery Rules

1. Build in stages. Do not start the next major module before the current foundation is stable.
2. Preserve clean boundaries between desktop client, shared contracts, server logic, and infrastructure concerns.
3. Keep modules isolated: Auth, Users, Friends, Messaging, Servers, Channels, Voice, Settings, Notifications.
4. Prefer additive changes over rewrites. Foundation decisions should make later modules easier to add.
5. Every stage must leave the repo buildable and documented.
6. Never claim a stage is complete until backend, client, and required infrastructure are verified locally.
7. Stage 1 is closed only after PostgreSQL connectivity, migrations, `/health`, `/swagger`, and `/api/health` are verified.
8. Stage 2 Auth is closed only after register, verify-email, login, and refresh are verified against the running backend.

## Architecture Rules

1. `shared/` contains transport contracts and cross-process primitives only.
2. `server/` owns API, realtime, application flow, domain entities, and persistence composition.
3. `client/` owns WPF UI, MVVM view models, client-side services, and session state.
4. Email, database, and external integrations stay behind abstractions.
5. Avoid feature logic in `Program.cs` and code-behind.
6. Do not collapse features into a monolith or a single mega-file.

## Git Flow

1. Use feature branches.
2. Commit by stage or coherent vertical slice.
3. Open a pull request after pushing a finished stage.
4. Do not squash architecture decisions into undocumented commits.
