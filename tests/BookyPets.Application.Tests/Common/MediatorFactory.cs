using BookyPets.Api;
using BookyPets.Application.Common.Interfaces;
using BookyPets.Infrastructure.Common.Persistence;
using BookyPets.Shared.Mediator.Abstractions;
using Common.Tests.TestUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BookyPets.Application.Tests.Common;

public class MediatorFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    private SqliteTestDatabase _testDatabase = null!;
    private readonly List<IServiceScope> _scopes = [];

    public FakeCurrentReaderProvider FakeCurrentReaderProvider { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _testDatabase = SqliteTestDatabase.CreateAndInitialize();

        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["Database:MigrateOnStartup"] = "false",
                ["Database:SeedOnStartup"] = "false",
            }));

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<DbContextOptions<BookyPetsDbContext>>()
                .AddDbContext<BookyPetsDbContext>((sp, options) =>
                    options.UseSqlite(_testDatabase.Connection));

            FakeCurrentReaderProvider = new FakeCurrentReaderProvider();
            services.RemoveAll<ICurrentReaderProvider>();
            services.AddSingleton<ICurrentReaderProvider>(FakeCurrentReaderProvider);
        });
    }

    public IMediator CreateMediator()
    {
        var serviceScope = Services.CreateScope();
        _scopes.Add(serviceScope);

        _testDatabase.ResetDatabase();
        FakeCurrentReaderProvider.Reset();

        return serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    }

    public TestScenario CreateScenario()
    {
        var mediator = CreateMediator();

        return new TestScenario(mediator, FakeCurrentReaderProvider);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    Task IAsyncLifetime.DisposeAsync()
    {
        foreach (var scope in _scopes)
        {
            scope.Dispose();
        }

        _testDatabase.Dispose();

        return Task.CompletedTask;
    }
}
