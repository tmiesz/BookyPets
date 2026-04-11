using BookyPets.Application.Common.Interfaces;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Commands.AcquireBook;

public class AcquireBookCommandHandler : IHandler<AcquireBookCommand, Result<Guid>>
{
    private readonly IBooksRepository _booksRepository;
    private readonly IReadersRepository _readersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentReaderProvider _currentReaderProvider;

    public AcquireBookCommandHandler(IBooksRepository booksRepository, IReadersRepository readersRepository, IUnitOfWork unitOfWork, ICurrentReaderProvider currentReaderProvider)
    {
        _booksRepository = booksRepository;
        _readersRepository = readersRepository;
        _unitOfWork = unitOfWork;
        _currentReaderProvider = currentReaderProvider;
    }

    public async Task<Result<Guid>> HandleAsync(AcquireBookCommand request, CancellationToken cancellationToken = default)
    {
        var currentReader = _currentReaderProvider.GetCurrentReader();

        var book = await _booksRepository.GetBookAsync(request.BookId);

        if (book is null)
            return new Error(ErrorType.NotFound, "BookNotFound", "Book was not found");

        var reader = await _readersRepository.GetByIdAsync(currentReader.Id);

        if (reader is null)
            return new Error(ErrorType.NotFound, "ReaderNotFound", "Reader was not found");

        var result = reader.AcquireBook(book.Id);

        if (!result.IsSuccess)
            return result.Error;

        await _readersRepository.UpdateReaderAsync(reader);
        await _unitOfWork.CommitChangesAsync();

        return result.Value;
    }
}
