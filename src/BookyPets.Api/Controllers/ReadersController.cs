using BookyPets.Api.Common;
using BookyPets.Application.Readers.Commands.AcquireBook;
using BookyPets.Application.Readers.Commands.AcquirePet;
using BookyPets.Application.Readers.Commands.ChangeAccountType;
using BookyPets.Application.Readers.Queries.GetProgress;
using BookyPets.Application.Readers.Queries.GetReader;
using BookyPets.Application.Readers.Queries.GetReaderBooks;
using BookyPets.Application.Readers.Queries.GetReaderPets;
using BookyPets.Contracts.Books;
using BookyPets.Contracts.Pets;
using BookyPets.Contracts.Readers;
using BookyPets.Shared.Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookyPets.Api.Controllers;

[Authorize]
[Route("[controller]")]
public class ReadersController(IMediator _mediator) : ApiController
{
    [HttpGet("{readerId:guid}")]
    public async Task<IActionResult> GetReader(Guid readerId)
    {
        var query = new GetReaderQuery(readerId);

        var getReaderResult = await _mediator.SendAsync(query);

        return getReaderResult.Match(
            reader => Ok(new ReaderResponse(reader.Id, reader.FirstName, DtoConverter.ToDto(reader.AccountType))),
            Problem);
    }

    [HttpPatch("{readerId}/account")]
    public async Task<IActionResult> ChangeAccountType(Guid readerId, ChangeAccountTypeRequest request)
    {
        if (!DtoConverter.TryToDomain(request.AccountType, out var domainAccountType))
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Invalid account type");
        }

        var command = new ChangeAccountTypeCommand(readerId, domainAccountType);

        var changeAccountTypeResult = await _mediator.SendAsync(command);

        return changeAccountTypeResult.Match(
            reader => Ok(new ReaderResponse(reader.Id, reader.FirstName, DtoConverter.ToDto(reader.AccountType))),
            Problem);
    }

    [HttpPost("pets/{petId:guid}/acquire")]
    public async Task<IActionResult> AcquirePet(Guid petId)
    {
        var command = new AcquirePetCommand(petId);

        var acquirePetResult = await _mediator.SendAsync(command);

        return acquirePetResult.Match(
            NoContent,
            Problem);
    }

    [HttpGet("pets")]
    public async Task<IActionResult> GetPets()
    {
        var query = new GetReaderPetsQuery();

        var getPetsResult = await _mediator.SendAsync(query);

        return getPetsResult.Match(
            pets => Ok(pets.Select(pet => new PetResponse(
                pet.Id,
                pet.Name,
                DtoConverter.ToDto(pet.Species),
                IconResolver.Species.Resolve(pet.Species),
                pet.FavouriteGenre is not null ? DtoConverter.ToDto(pet.FavouriteGenre) : null,
                pet.Level))),
            Problem);
    }

    [HttpPost("books/{bookId:guid}/acquire")]
    public async Task<IActionResult> AcquireBook(Guid bookId)
    {
        var command = new AcquireBookCommand(bookId);

        var acquireBookResult = await _mediator.SendAsync(command);

        return acquireBookResult.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpGet("books")]
    public async Task<IActionResult> GetBooks()
    {
        var query = new GetReaderBooksQuery();

        var getBooksResult = await _mediator.SendAsync(query);

        return getBooksResult.Match(
            books => Ok(books.Select(book => new BookResponse(
                book.Id,
                book.Title,
                book.Author,
                DtoConverter.ToDto(book.Genre),
                IconResolver.Genre.Resolve(book.Genre),
                book.PageCount))),
            Problem);
    }

    [HttpGet("progress/{progressId:guid}")]
    public async Task<IActionResult> GetProgress(Guid progressId)
    {
        var query = new GetProgressQuery(progressId);

        var getProgressResult = await _mediator.SendAsync(query);

        return getProgressResult.Match(
            progress => Ok(new ProgressResponse(progress.Id, progress.BookId, progress.CurrentPage, progress.TotalPages)),
            Problem
        );
    }
}
