using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Queries.GetReaderLibrary;

public record GetReaderLibraryQuery(int? Limit = null, int? Offset = null) : IRequest<Result<List<ReaderLibraryEntry>>>;
