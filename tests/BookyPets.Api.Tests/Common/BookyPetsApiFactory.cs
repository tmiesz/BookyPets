using System.Net.Http.Headers;
using BookyPets.Infrastructure.Common.Persistence;
using Common.Tests.TestConstants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BookyPets.Api.Tests.Common;

public class BookyPetsApiFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    private const string TestOrJwtScheme = "TestOrJwt";

    private SqliteTestDatabase _testDatabase = null!;

    public HttpClient HttpClient { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _testDatabase = SqliteTestDatabase.CreateAndInitialize();

        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(
            new Dictionary<string, string?> { ["Database:MigrateAndSeedOnStartup"] = "false" }));

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<DbContextOptions<BookyPetsDbContext>>()
                .AddDbContext<BookyPetsDbContext>((sp, options) =>
                    options.UseSqlite(_testDatabase.Connection));

            services.AddAuthentication(TestOrJwtScheme)
                .AddPolicyScheme(TestOrJwtScheme, TestOrJwtScheme, options =>
                    options.ForwardDefaultSelector = context =>
                        context.Request.Headers.ContainsKey(TestAuthHandler.UserIdHeader)
                            ? TestAuthHandler.SchemeName
                            : JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
        });
    }

    public Task InitializeAsync()
    {
        HttpClient = CreateClientFor(Constants.Reader.Id, Constants.Reader.Permissions, Constants.Reader.Roles);

        return Task.CompletedTask;
    }

    public new Task DisposeAsync()
    {
        _testDatabase.Dispose();

        return Task.CompletedTask;
    }

    public void ResetDatabase()
    {
        _testDatabase.ResetDatabase();
    }

    public HttpClient CreateAnonymousClient() => CreateClient();

    public HttpClient CreateClientFor(
        Guid readerId,
        IEnumerable<string>? permissions = null,
        IEnumerable<string>? roles = null)
    {
        var client = CreateClient();

        client.DefaultRequestHeaders.Add(TestAuthHandler.UserIdHeader, readerId.ToString());

        var permissionList = permissions?.ToList() ?? [];
        if (permissionList.Count > 0)
            client.DefaultRequestHeaders.Add(TestAuthHandler.PermissionsHeader, string.Join(',', permissionList));

        var roleList = roles?.ToList() ?? [];
        if (roleList.Count > 0)
            client.DefaultRequestHeaders.Add(TestAuthHandler.RolesHeader, string.Join(',', roleList));

        return client;
    }

    public HttpClient CreateReaderClient(Guid readerId) =>
        CreateClientFor(readerId, Constants.Reader.Permissions);

    public HttpClient CreateBearerClient(string token)
    {
        var client = CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }
}
