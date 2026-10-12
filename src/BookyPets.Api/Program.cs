using BookyPets.Api;
using BookyPets.Application;
using BookyPets.Infrastructure;

var BookyPetsWebOrigin = "_bookyPetsWebOrigin";

var builder = WebApplication.CreateBuilder(args);
{
    var allowedOrigins = builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>() ?? ["http://localhost:5173"];

    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment())
        .AddPresentation()
        .AddCors(opts => opts.AddPolicy(name: BookyPetsWebOrigin,
                    policy =>
                    {
                        policy.WithOrigins(allowedOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    })
                );
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment() &&
        app.Configuration.GetValue<bool>("Database:MigrateAndSeedOnStartup"))
    {
        await app.Services.InitializeDevelopmentDatabaseAsync();
    }

    app.UseExceptionHandler();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseCors(BookyPetsWebOrigin);

    app.UseAuthentication();
    app.UseAuthorization();
    app.AddInfrastructureMiddleware();

    app.MapControllers();

    app.Run();
}
