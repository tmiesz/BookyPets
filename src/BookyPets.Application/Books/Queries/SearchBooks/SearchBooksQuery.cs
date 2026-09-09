using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Books.Queries.SearchBooks;

public record SearchBooksQuery(string? Search = null) : IRequest<Result<List<Book>>>;
