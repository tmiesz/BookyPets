# BookyPets

A reading tracker with a small game layer. Add books to your library, time your reading sessions, and the pages you read earn experience for a virtual pet.

This is a personal portfolio project, built to show a .NET backend using Clean Architecture and DDD ideas, with a React + TypeScript frontend.

## Highlights

- **Clean Architecture + DDD:** Domain, Application, Infrastructure and API layers, with aggregates (`Reader`, `Book`, `Progress`, `Pet`, `Session`), domain events and private-state EF mapping.
- **CQRS-style use cases:** one command or query per handler, run through a pipeline of validation and authorization behaviours.
- **Hand-built building blocks:** a mediator, a `Result` type, a validator DSL and smart enums, written from scratch to learn how they work (see [Custom implementations](#custom-implementations)).
- **Security:** JWT auth, BCrypt hashing, a fallback "authenticated only" policy, permission checks in the pipeline, and ownership checks so readers can only touch their own data.
- **Reading sessions:** the timer is derived from timestamps (no drift), with heartbeats and stale-session detection.
- **Tests against a real database:** use cases run through the real mediator on SQLite, with a fake clock for time-based rules.
- **Frontend:** React 19, strict TypeScript, TanStack Query, React Hook Form, route guards, and plain CSS with design tokens and BEM.
- **Tooling:** Docker Compose, Nix flake and EF Core migrations with seed data.

## How it works

Readers register, add books and pets from the catalogue, then start a session on a book (optionally with a pet). Completing a session with the number of pages read raises a domain event that gives the pet experience. The amount depends on pages and time, with a bonus when the book's genre matches the pet's favourite genre. Experience rolls over into levels. A reader can have one active session at a time.

## Tech stack

| Area            | Technology                                                                |
| --------------- | ------------------------------------------------------------------------- |
| Backend         | C#, .NET 10, ASP.NET Core controllers, JWT bearer                         |
| Database        | SQLite, EF Core (code-first migrations, seed data)                        |
| Frontend        | React 19, TypeScript, Vite, React Router, TanStack Query, React Hook Form |
| Testing         | xUnit, WebApplicationFactory, NSubstitute                                 |
| Dev environment | Docker Compose, Nix flake + direnv                                        |

## Architecture

```text
src/
├── BookyPets.Api             Controllers, DI wiring, middleware
├── BookyPets.Application     Commands, queries, handlers, validators, pipeline behaviours
├── BookyPets.Contracts       Request/response DTOs
├── BookyPets.Domain          Aggregates, domain events, domain errors
├── BookyPets.Infrastructure  EF Core, repositories, migrations, seeding, JWT, password hashing
├── BookyPets.Shared          Mediator, Result, Validator, SmartEnum
└── BookyPets.Web             React + TypeScript frontend

tests/
├── BookyPets.Domain.Tests
├── BookyPets.Application.Tests
├── BookyPets.Api.Tests
└── Common.Tests              Shared factories and test helpers
```

Dependencies point inward: `Api` and `Infrastructure` depend on `Application`, which depends on `Domain`. Aggregates reference each other by ID, and time is passed into domain methods so session behaviour is deterministic in tests. Controllers are thin: they send a request through the mediator and map the `Result` to an HTTP response.

## Custom implementations

These live in `BookyPets.Shared` and were built on purpose to understand how they work, not because they beat the established libraries.

| Component | What it is                                                                       |
| --------- | -------------------------------------------------------------------------------- |
| Mediator  | Request and notification dispatch with a behaviour pipeline, resolved through DI |
| `Result`  | Success/failure type with typed errors and `Match`                               |
| Validator | A small rule DSL: `RuleFor(x => x.Prop)...`                                      |
| SmartEnum | Class-based enums with per-member data                                           |

## Getting started

### Option A: Docker Compose

```bash
docker compose up --build
```

- Web app: http://localhost:5173
- API: http://localhost:5293

The database is created and seeded on startup and stored in a Docker volume. Set `JWT_SECRET` to override the demo secret.

### Option B: Run locally

You need the .NET 10 SDK and Node.js 22 (or `nix develop` / `direnv allow`, which provides both).

```bash
# API: migrates and seeds a SQLite database on first run in Development
cd src/BookyPets.Api
dotnet run

# Frontend
cd src/BookyPets.Web
npm install
npm run dev
```

Open http://localhost:5173. The API runs on http://localhost:5293. To point the frontend elsewhere, set `VITE_API_URL`.

### Demo accounts (seeded demo data)

| Account | Email                    | Password             |
| ------- | ------------------------ | -------------------- |
| Reader  | `reader@bookypets.local` | `DemoReader123!Pass` |
| Admin   | `admin@bookypets.local`  | `AdminDev123!Pass`   |

Seed data includes 10 books and 6 pets. You can also register your own account.

## Configuration

| Setting                                                | Notes                                                                                                                                                                           |
| ------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `JwtSettings:*`                                        | Secret (32+ characters), issuer, audience and expiry. The API validates them on startup. A dev secret ships in `appsettings.Development.json`; use a private one anywhere else. |
| `ConnectionStrings:BookyPets`                          | SQLite path. Defaults to `BookyPets.db` in the API folder, `/data/bookypets.db` in Docker.                                                                                      |
| `Database:MigrateOnStartup` / `Database:SeedOnStartup` | On in Development, off by default elsewhere. Docker Compose turns them on.                                                                                                      |
| `Cors:AllowedOrigins`                                  | Defaults to `http://localhost:5173`.                                                                                                                                            |

## Testing

```bash
dotnet test
```

| Project                       | Covers                                                                                          |
| ----------------------------- | ----------------------------------------------------------------------------------------------- |
| `BookyPets.Domain.Tests`      | Aggregate rules: sessions (fake clock), pet experience and levels, progress, readers            |
| `BookyPets.Application.Tests` | Use cases through the real mediator and SQLite, plus authorization and pipeline-order behaviour |
| `BookyPets.Api.Tests`         | HTTP-level tests of the controllers, including anonymous access and authorization failures      |

`requests/` has ready-made `.http` files for every endpoint, for trying the API by hand. Log in first and paste the token into `@token`.
