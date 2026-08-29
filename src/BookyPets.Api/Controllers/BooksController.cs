using BookyPets.Api.Common;
using BookyPets.Application.Books.Commands;
using BookyPets.Application.Books.Queries;
using BookyPets.Contracts.Books;
using BookyPets.Shared.Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookyPets.Api.Controllers;

[Authorize]
[Route("[controller]")]
public class BooksController(IMediator _mediator) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateBook(CreateBookRequest request)
    {
        if (!DtoConverter.TryToDomain(request.Genre, out var genre))
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Invalid genre type");
        }

        var command = new CreateBookCommand(request.Title, request.Author, genre, request.PageCount);

        var createBookResult = await _mediator.SendAsync(command);

        return createBookResult.Match(
            book => Ok(new BookResponse(book.Id, book.Title, book.Author, DtoConverter.ToDto(book.Genre), book.PageCount)),
            Problem
        );
    }

    [HttpGet("{bookId:guid}")]
    public async Task<IActionResult> GetBook(Guid bookId)
    {
        var query = new GetBookQuery(bookId);

        var getBookResult = await _mediator.SendAsync(query);

        return getBookResult.Match(
            book => Ok(new BookResponse(book.Id, book.Title, book.Author, DtoConverter.ToDto(book.Genre), book.PageCount)),
            Problem
        );
    }

    [HttpGet("")]
    public async Task<IActionResult> GetBooks()
    {
        var query = new GetBooksQuery();

        var getBooksResult = await _mediator.SendAsync(query);

        return getBooksResult.Match(
            books => Ok(books.Select(book => new BookResponse(
                book.Id,
                book.Title,
                book.Author,
                DtoConverter.ToDto(book.Genre),
                book.PageCount))),
            Problem
        );
    }
}
