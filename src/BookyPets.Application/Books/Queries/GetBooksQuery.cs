using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Books.Queries;

public record GetBooksQuery(string? Search = null) : IRequest<Result<IReadOnlyList<Book>>>;
