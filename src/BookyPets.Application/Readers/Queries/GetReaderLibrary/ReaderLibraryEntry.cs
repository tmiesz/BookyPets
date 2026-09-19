using BookyPets.Domain.BookAggregate;

namespace BookyPets.Application.Readers.Queries.GetReaderLibrary;

public record ReaderLibraryEntry(Book Book, Progress Progress);
