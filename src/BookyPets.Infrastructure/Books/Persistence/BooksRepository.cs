using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookyPets.Infrastructure.Books.Persistence;

public class BooksRepository(BookyPetsDbContext dbContext) : IBooksRepository
{
    private readonly BookyPetsDbContext _dbContext = dbContext;

    public async Task AddBookAsync(Book book)
    {
        await _dbContext.Books.AddAsync(book);
    }

    public async Task<Book?> GetBookAsync(Guid bookId)
    {
        return await _dbContext.Books.FindAsync(bookId);
    }

    public async Task<List<Book>> GetBooksAsync(string? search = null)
    {
        var books = _dbContext.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();

            var matchingGenres = Genre.List
                .Where(g => g.Name.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            books = books.Where(book =>
                    EF.Functions.Like(book.Title, $"%{searchTerm}%") ||
                    EF.Functions.Like(book.Author, $"%{searchTerm}%") ||
                    (book.Genre != null && matchingGenres.Contains(book.Genre)));
        }

        return await books.ToListAsync();
    }
}
