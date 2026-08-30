using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Books.Queries.GetBook;

public class GetBookQueryHandler(IBooksRepository booksRepository) : IHandler<GetBookQuery, Result<Book>>
{
    private readonly IBooksRepository _booksRepository = booksRepository;

    public async Task<Result<Book>> HandleAsync(GetBookQuery query, CancellationToken cancellationToken = default)
    {
        var book = await _booksRepository.GetBookAsync(query.BookId);

        if (book is null)
            return new Error(ErrorType.NotFound, "BookNotFound");

        return book;
    }
}

