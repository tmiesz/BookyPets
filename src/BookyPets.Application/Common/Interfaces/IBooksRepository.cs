using BookyPets.Domain.BookAggregate;

namespace BookyPets.Application.Common.Interfaces;

public interface IBooksRepository
{
    Task AddBookAsync(Book book);
    Task<Book?> GetBookAsync(Guid bookId);
}
