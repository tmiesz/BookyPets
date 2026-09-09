using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Queries.GetReaderBooks;

public class GetReaderBooksQueryHandler(
    ICurrentReaderProvider currentReaderProvider,
    IReadersRepository readersRepository,
    IBooksRepository booksRepository) : IHandler<GetReaderBooksQuery, Result<List<Book>>>
{
    private readonly IReadersRepository _readersRepository = readersRepository;
    private readonly IBooksRepository _booksRepository = booksRepository;
    private readonly ICurrentReaderProvider _currentReaderProvider = currentReaderProvider;

    public async Task<Result<List<Book>>> HandleAsync(GetReaderBooksQuery query, CancellationToken cancellationToken = default)
    {
        var currentReader = _currentReaderProvider.GetCurrentReader();

        var reader = await _readersRepository.GetByIdAsync(currentReader.Id);

        if (reader is null)
            return new Error(ErrorType.NotFound, "ReaderNotFound", "Reader was not found");

        var ownedBookIds = reader.GetBooks();

        var ownedBooks = await _booksRepository.GetBooksAsync(ownedBookIds);

        return ownedBooks;
    }
}
