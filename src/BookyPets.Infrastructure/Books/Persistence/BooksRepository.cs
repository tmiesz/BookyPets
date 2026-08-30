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
        var books = await _dbContext.Books.ToListAsync();

        if(!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim().ToLower();
            books = [.. books.Where(book => 
                    book.Title.ToLower().Contains(searchTerm) || 
                    book.Author.ToLower().Contains(searchTerm))];
        }

        return books;
    }
}
