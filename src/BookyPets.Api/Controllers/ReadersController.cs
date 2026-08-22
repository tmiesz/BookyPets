using BookyPets.Api.Common;
using BookyPets.Application.Readers.Commands.AcquireBook;
using BookyPets.Application.Readers.Commands.AcquirePet;
using BookyPets.Application.Readers.Commands.ChangeAccountType;
using BookyPets.Application.Readers.Queries.GetProgress;
using BookyPets.Application.Readers.Queries.GetReader;
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

        var command = new ChangeAccountTypeCommand(readerId, domainAccountType!);

        var changeAccountTypeResult = await _mediator.SendAsync(command);

        return changeAccountTypeResult.Match(
            reader => Ok(new ReaderResponse(reader.Id, reader.FirstName, DtoConverter.ToDto(reader.AccountType))),
            Problem);
    }

    [HttpPost("{readerId:guid}/pets/{petId:guid}")]
    public async Task<IActionResult> AcquirePet(Guid petId)
    {
        var command = new AcquirePetCommand(petId);

        var acquirePetResult = await _mediator.SendAsync(command);

        return acquirePetResult.Match(
            NoContent,
            Problem);
    }

    [HttpPost("{readerId:guid}/books/{bookId:guid}")]
    public async Task<IActionResult> AcquireBook(Guid bookId)
    {
        var command = new AcquireBookCommand(bookId);

        var acquireBookResult = await _mediator.SendAsync(command);

        return acquireBookResult.Match(
            _ => NoContent(),
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
