using BookyPets.Api;
using BookyPets.Application;
using BookyPets.Infrastructure;
using Microsoft.Extensions.FileProviders;

var BookyPetsWebOrigin = "_bookyPetsWebOrigin";

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddPresentation()
        .AddCors(opts => opts.AddPolicy(name: BookyPetsWebOrigin,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    })
                );
}

var app = builder.Build();
{
    app.UseExceptionHandler();
    app.AddInfrastructureMiddleware();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.UseCors(BookyPetsWebOrigin);

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
                    Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "icons")),
        RequestPath = "/icons"
    });

    app.Run();
}
