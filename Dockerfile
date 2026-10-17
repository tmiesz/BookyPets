FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["src/BookyPets.Api/BookyPets.Api.csproj", "src/BookyPets.Api/"]
COPY ["src/BookyPets.Application/BookyPets.Application.csproj", "src/BookyPets.Application/"]
COPY ["src/BookyPets.Contracts/BookyPets.Contracts.csproj", "src/BookyPets.Contracts/"]
COPY ["src/BookyPets.Infrastructure/BookyPets.Infrastructure.csproj", "src/BookyPets.Infrastructure/"]
COPY ["src/BookyPets.Domain/BookyPets.Domain.csproj", "src/BookyPets.Domain/"]
COPY ["src/BookyPets.Shared/BookyPets.Shared.csproj", "src/BookyPets.Shared/"]

RUN dotnet restore "src/BookyPets.Api/BookyPets.Api.csproj"

COPY src/BookyPets.Api/ src/BookyPets.Api/
COPY src/BookyPets.Application/ src/BookyPets.Application/
COPY src/BookyPets.Contracts/ src/BookyPets.Contracts/
COPY src/BookyPets.Infrastructure/ src/BookyPets.Infrastructure/
COPY src/BookyPets.Domain/ src/BookyPets.Domain/
COPY src/BookyPets.Shared/ src/BookyPets.Shared/

RUN dotnet publish "src/BookyPets.Api/BookyPets.Api.csproj" -c Release --no-restore -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

ENV ASPNETCORE_HTTP_PORTS=5293 \
    ConnectionStrings__BookyPets="Data Source=/data/bookypets.db" \
    Database__MigrateOnStartup=true

EXPOSE 5293

WORKDIR /app
COPY --from=build /app/publish .

RUN mkdir -p /data && chown $APP_UID /data
VOLUME /data
USER $APP_UID

ENTRYPOINT ["dotnet", "BookyPets.Api.dll"]
