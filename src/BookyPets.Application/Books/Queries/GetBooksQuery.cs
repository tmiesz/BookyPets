using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Books.Queries;

public record GetBooksQuery() : IRequest<Result<IReadOnlyList<Book>>>;
