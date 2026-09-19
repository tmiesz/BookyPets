using BookyPets.Application.Common.Interfaces;
using BookyPets.Application.Common.Models;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Queries.GetReaderLibrary;

public class GetReaderLibraryQueryHandler(
    ICurrentReaderProvider currentReaderProvider,
    IReadersRepository readersRepository,
    IProgressesRepository progressesRepository,
    IBooksRepository booksRepository) : IHandler<GetReaderLibraryQuery, Result<List<ReaderLibraryEntry>>>
{
    private readonly IReadersRepository _readersRepository = readersRepository;
    private readonly IProgressesRepository _progressesRepository = progressesRepository;
    private readonly IBooksRepository _booksRepository = booksRepository;
    private readonly ICurrentReaderProvider _currentReaderProvider = currentReaderProvider;

    public async Task<Result<List<ReaderLibraryEntry>>> HandleAsync(GetReaderLibraryQuery request, CancellationToken cancellationToken = default)
    {
        var currentReader = _currentReaderProvider.GetCurrentReader();

        var reader = await _readersRepository.GetByIdAsync(currentReader.Id);

        if (reader is null)
            return new Error(ErrorType.NotFound, "ReaderNotFound", "Reader was not found");

        var progresses = await _progressesRepository.GetProgressesAsync(reader.GetProgresses());

        var bookIds = progresses.Select(p => p.BookId).Distinct().ToList();

        var books = await _booksRepository.GetBooksAsync(bookIds);
        var booksById = books.ToDictionary(b => b.Id);

        var entries = progresses
            .Where(p => booksById.ContainsKey(p.BookId))
            .Select(p => new ReaderLibraryEntry(booksById[p.BookId], p))
            .ToList();

        return entries;
    }
}
