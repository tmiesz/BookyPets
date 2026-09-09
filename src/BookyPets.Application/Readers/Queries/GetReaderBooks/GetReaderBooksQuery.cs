using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Queries.GetReaderBooks;

public record GetReaderBooksQuery() : IRequest<Result<List<Book>>>;
