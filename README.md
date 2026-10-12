# BookyPets

A gamified reading tracker API where readers earn experience for their virtual pets by completing reading sessions.

A project exploring key concepts of Domain Driven Design and Clean Architecture.

- Built in C# / .NET 10, with ASP.NET Core Web API as the presentation layer
- Custom component implementations
- CQRS
- Error handling using the Result pattern
- EF Core with SQLite

## Running locally

The API needs a JWT secret of at least 32 characters. With the Nix flake, `nix develop` sets one for you. Without it, use user-secrets:

```bash
cd src/BookyPets.Api
dotnet user-secrets set "JwtSettings:Secret" "<your-own-secret-of-32+-characters>"
```

In Development the API applies migrations and seeds some books, pets and two accounts on startup:

| Role   | Email                    | Password             |
| ------ | ------------------------ | -------------------- |
| Admin  | `admin@bookypets.local`  | `AdminDev123!Pass`   |
| Reader | `reader@bookypets.local` | `DemoReader123!Pass` |

These credentials are for local development only.
