using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Books.Queries.GetBooks;

public record GetBooksQuery(string? Search = null) : IRequest<Result<List<Book>>>;
