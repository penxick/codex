# Testcord Architecture

## Strategic Shape

The repository is organized around deployable boundaries first and feature boundaries second.

- `client/Testcord.Desktop` is the desktop shell and future user-facing experience.
- `server/Testcord.Server` is the backend composition root for HTTP, SignalR, persistence, and feature orchestration.
- `shared/Testcord.Shared` carries contracts that travel across process boundaries.
- `infrastructure/` is reserved for external deployment assets, scripts, and integration-specific material that should not leak into domain logic.

## Server Internal Layers

- `Domain`: entities and invariants that represent the communication platform.
- `Application`: service registration and use-case orchestration entry points.
- `Infrastructure`: EF Core, email, and external provider implementations.
- `Presentation`: controllers and SignalR hubs.
- `Configuration`: strongly typed options.

## Desktop Internal Layers

- `Views`: XAML windows and reusable visual composition points.
- `ViewModels`: MVVM state and commands.
- `Services`: API, session, realtime, and client-side infrastructure.
- `Configuration`: desktop bootstrap and settings.

## Foundation Decisions

1. Shared contracts are versionable and transport-focused.
2. Realtime is introduced at the foundation level through SignalR hub registration.
3. Voice is represented architecturally from day one through call entities and signaling placeholders, even before media transport is fully implemented.
4. SMTP is abstracted immediately to keep Auth independent from delivery details.
5. The initial desktop shell mirrors Discord information architecture to reduce future UI churn.
