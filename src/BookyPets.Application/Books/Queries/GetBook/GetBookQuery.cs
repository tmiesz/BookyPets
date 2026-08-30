using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Books.Queries.GetBook;

public record GetBookQuery(Guid BookId) : IRequest<Result<Book>>;
