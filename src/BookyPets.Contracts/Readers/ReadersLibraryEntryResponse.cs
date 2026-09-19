using BookyPets.Contracts.Books;

namespace BookyPets.Contracts.Readers;

public record ReaderLibraryEntryResponse(BookResponse Book, ProgressResponse Progress);
