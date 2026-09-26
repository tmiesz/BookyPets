FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["src/BookyPets.Api/BookyPets.Api.csproj", "src/BookyPets.Api/"]
COPY ["src/BookyPets.Application/BookyPets.Application.csproj", "src/BookyPets.Application/"]
COPY ["src/BookyPets.Contracts/BookyPets.Contracts.csproj", "src/BookyPets.Contracts/"]
COPY ["src/BookyPets.Infrastructure/BookyPets.Infrastructure.csproj", "src/BookyPets.Infrastructure/"]
COPY ["src/BookyPets.Domain/BookyPets.Domain.csproj", "src/BookyPets.Domain/"]
COPY ["src/BookyPets.Shared/BookyPets.Shared.csproj", "src/BookyPets.Shared/"]

RUN dotnet restore 'src/BookyPets.Api/BookyPets.Api.csproj'

COPY src/ .
WORKDIR /src/BookyPets.Api
RUN dotnet build 'BookyPets.Api.csproj' -c Release -o /app/build

FROM build AS publish
RUN dotnet publish 'BookyPets.Api.csproj' -c Release -o /app/publish
 
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
ENV ASPNETCORE_HTTP_PORTS=5293
EXPOSE 5293
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "BookyPets.Api.dll"]

