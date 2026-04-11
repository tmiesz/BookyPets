using BookyPets.Domain.ReaderAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Queries.GetReader;

public record GetReaderQuery(Guid ReaderId) : IRequest<Result<Reader>>;
