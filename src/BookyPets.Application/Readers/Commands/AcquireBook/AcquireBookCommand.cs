using BookyPets.Application.Common.Authorization;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Commands.AcquireBook;

[Authorize(Permissions = "books:acquire")]
public record AcquireBookCommand(Guid BookId) : IRequest<Result<Guid>>;
