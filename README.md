# BookyPets

A gamified reading tracker API where readers earn experience for their virtual pets by completing reading sessions.

A project exploring key concepts of Domain Driven Design and Clean Architecture.

- Built in C# / .NET 10. Asp.Net Web Api as the presenation layer.
- Custom component implementations.
- CQRS.
- Error handling using Result pattern.
- EF Core with SQlite as database.

## JWT configuration (local dev)

`appsettings.json` ships with empty `JwtSettings` placeholders.

**If using the Nix flake:** already handled via `shellHook` (`JwtSettings__Secret` env var) - just replace the placeholder string in `flake.nix` with your own random 32+ character string.

**If not using the flake:** use dotnet user-secrets instead:

```bash
cd src/BookyPets.Api
dotnet user-secrets init
dotnet user-secrets set "JwtSettings:Secret" "random-string-at-least-32-characters-long"
dotnet user-secrets set "JwtSettings:Issuer" "BookyPets"
dotnet user-secrets set "JwtSettings:Audience" "BookyPets"
dotnet user-secrets set "JwtSettings:TokenExpirationInMinutes" "60"
```

Note: the value of the secret is a placeholder — replace it with your own, don't reuse it beyond local dev.
