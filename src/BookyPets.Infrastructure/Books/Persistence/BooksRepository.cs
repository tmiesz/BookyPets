using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Infrastructure.Common.Persistence;

namespace BookyPets.Infrastructure.Books.Persistence;

public class BooksRepository : IBooksRepository
{
    private readonly BookyPetsDbContext _dbContext;

    public BooksRepository(BookyPetsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddBookAsync(Book book)
    {
        await _dbContext.Books.AddAsync(book);
    }

    public async Task<Book?> GetBookAsync(Guid bookId)
    {
        return await _dbContext.Books.FindAsync(bookId);
    }
}
