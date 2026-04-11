using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Books.Commands;

public class CreateBookCommandHandler : IHandler<CreateBookCommand, Result<Book>>
{
    private readonly IBooksRepository _booksRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(IBooksRepository booksRepository, IUnitOfWork unitOfWork)
    {
        _booksRepository = booksRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Book>> HandleAsync(CreateBookCommand request, CancellationToken cancellationToken = default)
    {
        var book = new Book(
            title: request.Title,
            author: request.Author,
            genre: request.Genre,
            pageCount: request.PageCount);

        await _booksRepository.AddBookAsync(book);
        await _unitOfWork.CommitChangesAsync();

        return book;
    }
}
