using System.Text;
using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Infrastructure.Authentication.PasswordHasher;
using BookyPets.Infrastructure.Authentication.TokenGenerator;
using BookyPets.Infrastructure.Books.Persistence;
using BookyPets.Infrastructure.Common.Persistence;
using BookyPets.Infrastructure.Pets.Persistence;
using BookyPets.Infrastructure.Progresses.Persistence;
using BookyPets.Infrastructure.Readers.Persistence;
using BookyPets.Infrastructure.Services;
using BookyPets.Infrastructure.Sessions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookyPets.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment)
    {
        return services
            .AddAuthentication(configuration, isDevelopment)
            .AddPersistence(configuration);

    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("BookyPets") ?? "Data Source=BookyPets.db";

        services.AddDbContext<BookyPetsDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IReadersRepository, ReadersRepository>();
        services.AddScoped<IPetsRepository, PetsRepository>();
        services.AddScoped<IBooksRepository, BooksRepository>();
        services.AddScoped<ISessionsRepository, SessionsRepository>();
        services.AddScoped<IProgressesRepository, ProgressesRepository>();

        services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<BookyPetsDbContext>());

        return services;
    }

    public static IServiceCollection AddAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.Section, jwtSettings);

        services.AddSingleton<IValidateOptions<JwtSettings>>(new JwtSettingsValidator(isDevelopment));
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.Section))
            .ValidateOnStart();

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromMinutes(1),
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            });

        return services;
    }
}
