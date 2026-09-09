using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Books.Queries.SearchBooks;

public class SearchBooksQueryHandler(IBooksRepository booksRepository) : IHandler<SearchBooksQuery, Result<List<Book>>>
{
    private readonly IBooksRepository _booksRepository = booksRepository;

    public async Task<Result<List<Book>>> HandleAsync(SearchBooksQuery request, CancellationToken cancellationToken = default)
    {
        var books = await _booksRepository.SearchBooksAsync(request.Search);

        return books;
    }
}

