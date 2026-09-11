using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Queries.GetReaderProgresses;

public record GetReaderProgressesQuery() : IRequest<Result<List<Progress>>>;

