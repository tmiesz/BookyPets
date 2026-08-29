using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Books.Queries;

public class GetBooksQueryHandler(IBooksRepository booksRepository) : IHandler<GetBooksQuery, Result<IReadOnlyList<Book>>>
{
    private readonly IBooksRepository _booksRepository = booksRepository;

    public async Task<Result<IReadOnlyList<Book>>> HandleAsync(GetBooksQuery request, CancellationToken cancellationToken = default)
    {
        var books = await _booksRepository.GetBooksAsync();

        return books.ToList();
    }
}

