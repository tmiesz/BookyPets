using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Domain.PetAggregate;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookyPets.Infrastructure;

public static class DatabaseInitializer
{
    // Development credentials. Don't use outside local development.
    public const string AdminEmail = "admin@bookypets.local";
    public const string AdminPassword = "AdminDev123!Pass";
    public const string DemoReaderEmail = "reader@bookypets.local";
    public const string DemoReaderPassword = "DemoReader123!Pass";

    public static async Task InitializeDevelopmentDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<BookyPetsDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        await dbContext.Database.MigrateAsync();

        await SeedReadersAsync(dbContext, passwordHasher);
        await SeedBooksAsync(dbContext);
        await SeedPetsAsync(dbContext);

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedReadersAsync(BookyPetsDbContext dbContext, IPasswordHasher passwordHasher)
    {
        if (!await dbContext.Readers.AnyAsync(r => r.Email == AdminEmail))
        {
            var admin = new Reader("Admin", "User", AdminEmail, Hash(passwordHasher, AdminPassword));
            admin.AssignRole(Role.Admin);

            await dbContext.Readers.AddAsync(admin);
        }

        if (!await dbContext.Readers.AnyAsync(r => r.Email == DemoReaderEmail))
        {
            var reader = new Reader("Demo", "Reader", DemoReaderEmail, Hash(passwordHasher, DemoReaderPassword));

            await dbContext.Readers.AddAsync(reader);
        }
    }

    private static async Task SeedBooksAsync(BookyPetsDbContext dbContext)
    {
        if (await dbContext.Books.AnyAsync())
            return;

        Book[] books =
        [
            new("A Brief History of Time", "Stephen Hawking", Genre.Science, 212),
            new("Clean Code", "Robert C. Martin", Genre.Technology, 464),
            new("The Pragmatic Programmer", "Andrew Hunt", Genre.Technology, 352),
            new("Meditations", "Marcus Aurelius", Genre.Philosophy, 254),
            new("Sapiens", "Yuval Noah Harari", Genre.History, 443),
            new("Thinking, Fast and Slow", "Daniel Kahneman", Genre.Psychology, 499),
            new("The Great Gatsby", "F. Scott Fitzgerald", Genre.Fiction, 180),
            new("The Hobbit", "J.R.R. Tolkien", Genre.Fantasy, 310),
            new("Long Walk to Freedom", "Nelson Mandela", Genre.Biography, 656),
            new("The Art of Learning", "Josh Waitzkin", Genre.Educational, 288),
        ];

        await dbContext.Books.AddRangeAsync(books);
    }

    private static async Task SeedPetsAsync(BookyPetsDbContext dbContext)
    {
        if (await dbContext.Pets.AnyAsync())
            return;

        Pet[] pets =
        [
            new("Biscuit", Species.Dog, Genre.Fiction),
            new("Luna", Species.Bird, Genre.Fantasy),
            new("Orion", Species.Whale, Genre.Science),
            new("Maple", Species.Bear, Genre.History),
            new("Zuri", Species.Lion, Genre.Biography),
            new("Fern", Species.Butterfly, null),
        ];

        await dbContext.Pets.AddRangeAsync(pets);
    }

    private static string Hash(IPasswordHasher passwordHasher, string password)
    {
        var result = passwordHasher.HashPassword(password);

        return result.IsSuccess
            ? result.Value
            : throw new InvalidOperationException($"Seed password was rejected: {result.Error.Description}");
    }
}
